using Newtonsoft.Json.Linq;

namespace ACIPL.Template.Server.Models
{
    public class SyncRequest
    {
        public int EmployeeId { get; set; }
        public string DataFilePath { get; set; }
        public string TableName { get; set; }
        public string StoredProcedureName { get; set; }
        public string HeaderName { get; set; }
        public string UnzipFileName { get; set; }
        public string DoneSyncFolderPath { get; set; }
        public JObject ReadJsonNewFolder { get; set; }
        public string ResponseSyncFolderPath { get; set; }
        public string ResponseFilePath { get; set; }

    }
}
