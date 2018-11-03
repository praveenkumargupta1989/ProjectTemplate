namespace ACIPL.Template.Server.Models.Constants
{
    public static class AppConstant
    {
        public static readonly string PharmawareSMDPath = "RootPath";
        #region Check Valid Input Parameter
        public const char SplitValue = '|';
        public const char InvalidSplitValue = '\0';
        public const string InvalidInputParameter = "Invalid Input Parameter";

        #endregion

        #region Sync
        public const string SyncPath = "SyncPath";
        public const string SyncNew = "New\\";
        public const string SyncDone = "Done\\";
        public const string SyncError = "Error\\";
        public const string SyncResponse = "Response\\";
        public const string dataType = " -c";
        public const string overrideDelimeter = " -t";
        public const string SyncSepration = "\"" + DataSeparator + "\"";
        public const string TempTable = "Temp";

        //Get Data From Webconfig
        public const string SyncDBName = "SyncDBName";
        public const string SyncOutDBName = "SyncOutDBName";
        public const string SyncDBNameMaster = "Pharmaware";
        public const string SyncDBUsername = "SyncDBUserName";
        public const string SyncDBPassword = "SyncDBPassword";
        public const string SyncBcpExeFilePath = "SyncBcpExeFilePath";
        public const string ServerIPAddress = "ServerIPAddress";
        #endregion

        #region FileExtension
        public const string ZipExtension = ".zip";
        public const string TextExtension = ".txt";
        public const string JsonExtension = ".json";
        public const string DataSeparator = "|";
        #endregion

        #region SyncStatus
        public const string SyncStatusSuccessfull = "S";
        public const string SyncStatusFailed = "F";
        public const string SyncStatusError = "E";
        #endregion
    }
}
