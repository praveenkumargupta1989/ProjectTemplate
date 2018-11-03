using ACIPL.Template.Core.Utilities;
using ACIPL.Template.Server.Models;
using System;
using System.IO;
using System.Web;
using System.Web.Http;

namespace ACIPL.Template.Server.Services.Controllers
{
    public class UploadController : BaseApiController
    {
        private readonly IConfigurationManager configurationManager;
        private readonly IFileManager fileManger;

        public UploadController(IConfigurationManager configurationManager, IFileManager fileManger)
        {
            this.configurationManager = configurationManager;
            this.fileManger = fileManger;
        }
        [HttpPost]
        public IHttpActionResult RequestForAssistance(Upload entity)
        {
            if ((!(string.IsNullOrEmpty(entity.fileName)) && (!string.IsNullOrEmpty(entity.fileData))))
            {
                var rootPath = configurationManager.GetConfigurationValue("RootPath");
                var backupPath = rootPath + configurationManager.GetConfigurationValue("RequestForAssistancePath");
                var backupPathFile = backupPath + entity.fileName.ToString();

                //store file in backup folder
                fileManger.CreateDirectory(backupPath, true);
                fileManger.Delete(backupPathFile);

                using (FileStream fs = File.Create(backupPath + entity.fileName))
                {
                    foreach (byte value in Convert.FromBase64String(entity.fileData))
                    {
                        fs.WriteByte(value);
                    }
                    fs.Close();
                }
                return Ok(true);
            }
            return Ok(false);
        }

        [HttpPost]
        public IHttpActionResult LogFile(Upload entity)
        {

            if ((!(string.IsNullOrEmpty(entity.fileName)) && (!string.IsNullOrEmpty(entity.fileData))))
            {
                var backupPath = HttpContext.Current.Server.MapPath(configurationManager.GetConfigurationValue("LogFilePath"));
                var backupPathFile = backupPath + entity.fileName.ToString();
                fileManger.CreateDirectory(backupPath, true);
                fileManger.Delete(backupPathFile);

                //store file in backup folder

                using (FileStream fs = File.Create(backupPath + entity.fileName))
                {
                    foreach (byte value in Convert.FromBase64String(entity.fileData))
                    {
                        fs.WriteByte(value);
                    }
                    fs.Close();
                }
                return Ok(true);
            }
            return Ok(false);
        }
    }
}
