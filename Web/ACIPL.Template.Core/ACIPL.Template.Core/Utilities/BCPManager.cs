using ACIPL.Template.Core.Logging;
using System.Diagnostics;

namespace ACIPL.Template.Core.Utilities
{
    /*
     * "C:\Program Files\Microsoft SQL Server\Client SDK\ODBC\110\Tools\Binn\bcp" 
     * ManagerDailyCallReportingOtherFieldWorkTemp in C:\Applications\Catalyst\CatalystFiles\Sync\Done\940201832016152.json 
     * -S"192.168.0.100" -dCatalyst -Ucatalyst -PCatalyst -c -t"|"
     */
    public interface IBCPManager
    {
        bool In(string tableName, string fileToUpload, string serverIPAdderess, string serverName, string userName, string password, string dataType, string overrideDelimeter, string seprator, string workingDirectory);
        BCPResponse In(BCPReqest bcpRequest);
        BCPResponse QueryOut(BCPReqest bcpRequest);
        bool QueryOut(string query, string fileToUpload, string serverIPAdderess, string serverName, string userName, string password, string dataType, string overrideDelimeter, string seprator, string workingDirectory);
    }
    public class BCPManager : IBCPManager
    {
        private readonly ILogger Logger = LoggerFactory.GetLogger();
        private readonly IFileManager fileManager;
        private readonly ISQLConnectionStringBuilder sqlConnectionStringBuilder;

        public BCPManager(IFileManager fileManager, ISQLConnectionStringBuilder sqlConnectionStringBuilder)
        {
            this.fileManager = fileManager;
            this.sqlConnectionStringBuilder = sqlConnectionStringBuilder;
        }

        public bool In(string tableName, string fileToUpload, string serverIPAdderess, string serverName, string userName,
                        string password, string dataType, string overrideDelimeter, string seprator, string workingDirectory)
        {
            Logger.Info("BCP Manager In Started");
            string cmdExe = tableName + " in " + fileToUpload + " -S\" " + serverIPAdderess
                            + "\" -d " + serverName + " -U " + userName
                            + " -P " + password + dataType + overrideDelimeter + seprator;

            Logger.Debug("Working Directory - " + workingDirectory);
            Logger.Debug("Command - " + cmdExe);

            //BCP File Upload
            FileUpload(fileToUpload, workingDirectory, cmdExe);

            Logger.Info("BCP Manager In Completed");
            return true;
        }

        public bool QueryOut(string query, string fileToUpload, string serverIPAdderess, string serverName, string userName,
                            string password, string dataType, string overrideDelimeter, string seprator, string workingDirectory)
        {
            Logger.Info("BCP Manager Query Out Started");

            string cmdExe = @"""" + query + @""" queryout " + fileToUpload + " -S\"" + serverIPAdderess
                            + "\" -d " + serverName + " -U " + userName
                            + " -P " + password + dataType + overrideDelimeter + seprator;

            Logger.Debug("Working Directory - " + workingDirectory);
            Logger.Debug("Command - " + cmdExe);

            //BCP File Upload
            FileUpload(fileToUpload, workingDirectory, cmdExe);

            Logger.Info("BCP Manager Query Out Completed");
            return true;
        }
        private void FileUpload(string fileToUpload, string workingDirectory, string cmdExe)
        {
            var path = fileToUpload + ".bat";
            var data = string.Format(@"""{0}""\bcp {1}", workingDirectory, cmdExe);

            fileManager.Create(path);
            fileManager.TextWritter(path, data);

            Logger.Debug("Batch File Path - " + path);
            Logger.Debug("Batch File Data - " + data);

            int exitCode;
            ProcessStartInfo processInfo;
            Process process;

            processInfo = new ProcessStartInfo("cmd.exe", "/c " + path);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;

            // *** Redirect the output ***
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;
            process = Process.Start(processInfo);
            process.WaitForExit();

            // *** Read the streams ***
            // Warning: This approach can lead to deadlocks, see Edit #2
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            Logger.Info("Process Output - " + output);
            Logger.Info("Process Error - " + error);

            exitCode = process.ExitCode;
            Logger.Info(exitCode);
            process.Close();
        }
        public BCPResponse In(BCPReqest bcpRequest)
        {
            Logger.Debug("BCP Manager In Started");
            sqlConnectionStringBuilder.Build(bcpRequest.ConnectionString);

            string command = PrepareBCPCommand(bcpRequest, "in");
            var bcpReposnse = PrepareAndExecuteBCPBatch(bcpRequest.FilePath + ".bat", command);

            fileManager.Delete(bcpRequest.FilePath + ".bat");

            Logger.Debug("Command - " + command);
            Logger.Debug("BCP Manager In Completed");
            return bcpReposnse;
        }
        public BCPResponse QueryOut(BCPReqest bcpRequest)
        {
            Logger.Debug("BCP Manager QueryOut Started");
            sqlConnectionStringBuilder.Build(bcpRequest.ConnectionString);


            string command = PrepareBCPCommand(bcpRequest, "queryout");
            var bcpReposnse = PrepareAndExecuteBCPBatch(bcpRequest.ResponseFilePath + ".bat", command);
            //  fileManager.Delete(bcpRequest.ResponseFilePath + ".bat");

            Logger.Debug("Command - " + command);
            Logger.Debug("BCP Manager QueryOut Completed");
            return bcpReposnse;
        }
        private string PrepareBCPCommand(BCPReqest bcpRequest, string commandType)
        {
            string command = string.Format("\"{0}\\bcp\" \"{1}\" {2} \"{3}\" -S\"{4}\" -d{5} -U{6} -P{7} {8} {9} {10}",
                bcpRequest.BCPUtilityPath,//0
               bcpRequest.SqlObject, //1
               commandType,//2
               commandType.ToLower() == "in" ? bcpRequest.FilePath : bcpRequest.ResponseFilePath,//3
               sqlConnectionStringBuilder.ServerName, //4
               sqlConnectionStringBuilder.DatabaseName,//5 
               sqlConnectionStringBuilder.UserId,//6
               sqlConnectionStringBuilder.Password,//7
               bcpRequest.DataType, //8
               bcpRequest.OverrideDelimiter, //9
               bcpRequest.Seperator//10
               );

            return command;
        }
        private BCPResponse PrepareAndExecuteBCPBatch(string batchFilePath, string command)
        {
            var bcpResponse = new BCPResponse();
            fileManager.Create(batchFilePath);
            fileManager.TextWritter(batchFilePath, command);

            Logger.Debug("Batch File Path - " + batchFilePath);
            Logger.Debug("Batch File Data - " + command);

            int exitCode;
            ProcessStartInfo processInfo;
            Process process;

            processInfo = new ProcessStartInfo("cmd.exe", "/c " + batchFilePath);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;

            // *** Redirect the output ***
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;
            process = Process.Start(processInfo);
            process.WaitForExit();

            // *** Read the streams ***
            // Warning: This approach can lead to deadlocks, see Edit #2
            bcpResponse.StandardOutput = process.StandardOutput.ReadToEnd();
            bcpResponse.StandardError = process.StandardError.ReadToEnd();
            Logger.Info("Process Output - " + bcpResponse.StandardOutput);
            Logger.Info("Process Error - " + bcpResponse.StandardError);
            exitCode = process.ExitCode;
            Logger.Info(exitCode);
            process.Close();
            return bcpResponse;
        }
    }
}
