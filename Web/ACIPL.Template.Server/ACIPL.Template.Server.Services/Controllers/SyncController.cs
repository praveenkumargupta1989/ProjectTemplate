using ACIPL.Template.Core.Utilities;
using ACIPL.Template.Server.Models;
using ACIPL.Template.Server.Models.Constants;
using ACIPL.Template.Server.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace ACIPL.Template.Server.Services.Controllers
{
    public class SyncController : BaseApiController
    {
        private readonly ISyncRepository syncRepository;
        private readonly IConfigurationManager configurationManager;
        private readonly IFileManager fileManager;
        private readonly IBCPManager bcpManager;
        private readonly IStreamWriterManager streamWriterManager;

        public SyncController(ISyncRepository syncRepository, IConfigurationManager configurationManager, IFileManager fileManager, IBCPManager bcpManager,
                                IStreamWriterManager streamWriterManager)
        {
            this.syncRepository = syncRepository;
            this.configurationManager = configurationManager;
            this.fileManager = fileManager;
            this.bcpManager = bcpManager;
            this.streamWriterManager = streamWriterManager;
        }

        [HttpGet]
        public IHttpActionResult GetSyncObjects()
        {
            var list = syncRepository.GetSyncTableOrder();
            if (list.Any())
            {
                return Ok(list.Select(x => new { Name = x.Name, Order = x.Order, ResponseData = x.ResponseData, SyncMethod = x.SyncMethod }));
            }
            return Ok(list);
        }

        [HttpPost]
        public IHttpActionResult SyncData([FromBody]string data)
        {

            string dataFilePath = "", doneSyncFolderPath = "", tableName = "", zipBase64 = "", headerName = "";

            var token = Request.Headers.GetValues("ACIPLToken").FirstOrDefault();
            var emp = SessionManager.Get(token) as Login;
            var src = DateTime.Now;
            var hm = string.Format("{0}{1}{2}{3}{4}{5}", src.Year, src.Month, src.Day, src.Hour, src.Minute, src.Second);
            string fileName = string.Format("{0}{1}{2}", emp.EmpId, tableName, hm);
            var newSyncFolderPath = string.Format("{0}{1}{2}", configurationManager.GetConfigurationValue(AppConstant.PharmawareSMDPath), configurationManager.GetConfigurationValue(AppConstant.SyncPath), AppConstant.SyncNew);
            var newFilePath = string.Format("{0}{1}{2}", newSyncFolderPath, fileName, AppConstant.ZipExtension);
            var syncDoneFolderPath = string.Format("{0}{1}{2}", configurationManager.GetConfigurationValue(AppConstant.PharmawareSMDPath), configurationManager.GetConfigurationValue(AppConstant.SyncPath), AppConstant.SyncDone);
            var syncDoneFilePath = string.Format("{0}{1}{2}", syncDoneFolderPath, fileName, AppConstant.ZipExtension);
            var syncResponseFolderPath = configurationManager.GetConfigurationValue(AppConstant.PharmawareSMDPath) + configurationManager.GetConfigurationValue(AppConstant.SyncPath) + AppConstant.SyncResponse;
            var syncResponseFilePath = string.Format("{0}{1}{2}", syncResponseFolderPath, fileName, AppConstant.TextExtension);
            var tblName = string.Empty;
            try
            {
                // Create Directories If they don't exist
                fileManager.CreateDirectory(newSyncFolderPath);
                fileManager.CreateDirectory(syncDoneFolderPath);
                fileManager.CreateDirectory(syncResponseFolderPath);

                //Create Directory And convert base64 to zipfolder
                CreateDirConvertBase64ToZip(newSyncFolderPath, newFilePath, data);

                //zip file will unzip Here
                var unzipFileName = fileManager.UnZip(newSyncFolderPath, newFilePath, fileName);
                var fileContent = File.ReadAllText(unzipFileName).Replace("null", "");
                var jsonData = JsonConvert.DeserializeObject<SyncTableType>(fileContent);
                var tableDetail = syncRepository.GetTableDetail(jsonData.Name);
                tblName = jsonData.Name;
                //Read data from unzip file and generate JSON object            
                doneSyncFolderPath = string.Format("{0}{1}{2}", configurationManager.GetConfigurationValue(AppConstant.PharmawareSMDPath), configurationManager.GetConfigurationValue(AppConstant.SyncPath), AppConstant.SyncDone);
                dataFilePath = string.Format("{0}{1}{2}", doneSyncFolderPath, fileName, AppConstant.JsonExtension);
                SyncRequest request = GetSyncRequestObj(dataFilePath, doneSyncFolderPath, tableDetail.TempTableName, headerName, emp, unzipFileName, tableDetail, null, syncResponseFolderPath, syncResponseFilePath);
                var resp = string.Empty;

                switch (tableDetail.SyncMethod)
                {
                    case ("OWFS"):
                        var qr = ReplaceextraWhiteSpace(GetSyncData(request.EmployeeId, tableDetail.Name, request.StoredProcedureName));
                        BCPQueryOut(qr, request, jsonData);
                        resp = ZipToBase64(syncResponseFilePath, syncDoneFolderPath);
                        break;
                    case ("OWFD"):
                        if (jsonData.Data.Count > 0)
                            BCPIn(jsonData, request);

                        break;
                    case ("TW"):
                        if (jsonData.Data.Count > 0)
                        {
                            var bcpInResult = BCPIn(jsonData, request);
                            if (bcpInResult)
                            {
                                fileManager.Move(newFilePath, syncDoneFilePath);
                                Directory.Delete(newSyncFolderPath + fileName, true);
                            }
                        }

                        qr = ReplaceextraWhiteSpace(GetSyncData(request.EmployeeId, tableDetail.Name, request.StoredProcedureName));
                        request.DataFilePath = syncResponseFilePath;
                        var bcpOutStatus = BCPQueryOut(qr, request, jsonData);
                        if (bcpOutStatus.IsSuccessful)
                        {
                            resp = ZipToBase64(syncResponseFilePath, syncDoneFolderPath);
                        }
                        else
                        {

                            var message = new StringBuilder()
                                .Append("Pharmaware Mobile Sync Failed on: ")
                                .Append(Request.RequestUri).Append("<br/>")
                                .Append("For Employee Id:").Append(JsonConvert.SerializeObject(emp)).Append("<br/>")
                                .Append("With Token Number: ").Append(token).Append("<br/>")
                                .Append("Table Name:").Append(tblName).Append("<br/>")
                                .Append("BCPOUTPUT StandardOutput:").Append(bcpOutStatus.StandardOutput).Append("<br/>")
                                .Append("BCPOUTPUT StandardError:").Append(bcpOutStatus.StandardError).Append("<br/>")
                                .ToString();
                            throw new CustomException(message);
                        }

                        break;
                }

                return Ok(resp);
            }
            catch (Exception e)
            {
                var message = new StringBuilder()
                .Append("Pharmaware Mobile Sync Failed on: ")
                .Append(Request.RequestUri).Append("<br/>")
                .Append("For Employee Id:").Append(JsonConvert.SerializeObject(emp)).Append("<br/>")
                .Append("With Token Number: ").Append(token).Append("<br/>")
                .Append("Table Name:").Append(tblName).Append("<br/>")
                .Append("Please find the data:").Append(data).Append("<br/>")
                .Append("Stack trace Of error is: ").Append(e.ToString()).Append("<br/>")
                .ToString();

                throw new CustomException(message);

            }
        }

        private string ZipToBase64(string syncResponseFilePath, string syncDoneFolderPath)
        {
            var zipFileName = syncDoneFolderPath + Path.GetFileNameWithoutExtension(syncResponseFilePath) + AppConstant.ZipExtension;


            //using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
            //{
            //    zip.AddFile(syncResponseFilePath, "");
            //    zip.Save(zipFileName);
            //}

            var base64EncodedData = Convert.ToBase64String(File.ReadAllBytes(zipFileName));

            return base64EncodedData;
        }

        private static SyncRequest GetSyncRequestObj(string dataFilePath, string doneSyncFolderPath, string tableName, string headerName, Login emp, string unzipFileName, SyncTableOrder tableDetail, JObject readJsonNewFolder, string syncResponseFolderPath, string syncResponseFilePath)
        {
            var syncrequest = new SyncRequest();
            syncrequest.DataFilePath = dataFilePath;
            syncrequest.DoneSyncFolderPath = doneSyncFolderPath;
            syncrequest.EmployeeId = emp.EmpId;
            syncrequest.HeaderName = headerName;
            syncrequest.TableName = tableName;
            syncrequest.UnzipFileName = unzipFileName;
            syncrequest.ReadJsonNewFolder = readJsonNewFolder;
            syncrequest.StoredProcedureName = tableDetail.StoredProcedureName;
            syncrequest.ResponseSyncFolderPath = syncResponseFolderPath;
            syncrequest.ResponseFilePath = syncResponseFilePath;
            return syncrequest;
        }

        [HttpPost]
        public IHttpActionResult SyncAckNack(SyncAckNack entity, HttpRequestMessage request)
        {
            Logger.Info("Sync > SyncAckNack Method Called");

            if (entity != null)
            {
                Login emp = ExtractEmployeeIdRequest(request);
                entity.EmployeeId = emp.EmpId;
                entity.AckNack = "Nack";
                if (!string.IsNullOrEmpty(entity.Status))
                {
                    if (entity.Status.ToLower() == "success")
                    {
                        Logger.Info("Sync > SyncAckNack > Status - Ack");
                        entity.AckNack = "Ack";
                    }
                    else if (entity.Status.ToLower() == "fail" || string.IsNullOrEmpty(entity.Status))
                    {
                        Logger.Info("Sync > SyncAckNack > Status - NAck");
                        entity.AckNack = "Nack";
                    }
                }

                var list = syncRepository.SyncAckNack(entity);
                if (list != null)
                {
                    return Ok(new { Name = list.Name, Status = list.AckNack });
                }
            }
            return Ok();
        }

        public Login ExtractEmployeeIdRequest(HttpRequestMessage request)
        {
            var token = Request.Headers.GetValues("ACIPLToken").FirstOrDefault();
            var emp = SessionManager.Get(token) as Login;
            return emp;
        }

        private string GetSyncData(int employeeId, string tableName, string storedProcedureName)
        {
            return syncRepository.SyncData(employeeId, tableName, storedProcedureName);
        }

        private void CreateDirConvertBase64ToZip(string newSyncFolderPath, string newFilePath, string data)
        {
            Logger.Info("Sync > SyncData > CreateDirConvertBase64ToZip Method Called");

            //Directroy doesn't exist then create new directory
            fileManager.CreateDirectory(newSyncFolderPath);
            fileManager.Delete(newFilePath);

            //Create zip file and save
            fileManager.Zip(newSyncFolderPath, newFilePath);

            //Read data from base64 file and store into created zip file
            streamWriterManager.StreamWriteFromBase64(newFilePath, data);
        }

        private bool BCPIn(SyncTableType syncTableType, SyncRequest request)
        {
            fileManager.CreateDirectory(request.DoneSyncFolderPath);

            var strData = string.Join("", syncTableType.Data.Select(x => x + AppConstant.DataSeparator + request.EmployeeId + Environment.NewLine));
            fileManager.Create(request.UnzipFileName);
            File.AppendAllText(request.UnzipFileName, strData);
            //fileManager.TextWritter(request.UnzipFileName, strData);
            var bcpRequest = GetBcpRequestObj(request.TableName, request);
            var bcpResponse = bcpManager.In(bcpRequest);

            return bcpResponse.IsSuccessful;
        }

        private BCPResponse BCPQueryOut(string query, SyncRequest request, SyncTableType syncTableType)
        {
            //dataFilePath            
            var bcpRequest = GetBcpRequestObj(query, request);
            var bcpResponse = bcpManager.QueryOut(bcpRequest);
            if (bcpResponse.IsSuccessful)
            {
                var allData = File.ReadAllText(bcpRequest.ResponseFilePath);
                syncTableType.Data = allData.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
                syncTableType.Status = AppConstant.SyncStatusSuccessfull;
                File.WriteAllText(bcpRequest.ResponseFilePath, JsonConvert.SerializeObject(syncTableType));
            }
            else if (!string.IsNullOrWhiteSpace(bcpResponse.StandardError))
                syncTableType.Status = AppConstant.SyncStatusError;
            else
            {
                syncTableType.Status = AppConstant.SyncStatusFailed;
            }
            return bcpResponse;
        }

        private BCPReqest GetBcpRequestObj(string sqlObject, SyncRequest request)
        {
            var bcpRequest = new BCPReqest();
            bcpRequest.BCPUtilityPath = configurationManager.GetConfigurationValue(AppConstant.SyncBcpExeFilePath);
            bcpRequest.ConnectionString = configurationManager.GetConnectionString("ConnectionString");
            bcpRequest.DataType = AppConstant.dataType;
            bcpRequest.FilePath = request.UnzipFileName;
            bcpRequest.OverrideDelimiter = AppConstant.overrideDelimeter;
            bcpRequest.Seperator = AppConstant.SyncSepration;
            bcpRequest.SqlObject = sqlObject;
            bcpRequest.ResponseFilePath = request.ResponseFilePath;

            return bcpRequest;
        }

        private string ReplaceextraWhiteSpace(string src)
        {
            var chars = src.ToCharArray().ToList();
            StringBuilder str = new StringBuilder();
            int whiteSpaceCount = 0;
            foreach (var ch in chars)
            {
                if (ch == ' ' && whiteSpaceCount == 0)
                {
                    whiteSpaceCount = whiteSpaceCount + 1;
                    str.Append(ch);
                }
                else if (ch == '\n' || ch == '\t' || ch == '\r')
                {
                    whiteSpaceCount = whiteSpaceCount + 1;
                    str.Append(' ');
                }
                else
                {
                    if (whiteSpaceCount > 1)
                        str.Append(" ");
                    str.Append(ch);
                    whiteSpaceCount = 0;
                }

            }
            return str.ToString();
        }



    }
}
