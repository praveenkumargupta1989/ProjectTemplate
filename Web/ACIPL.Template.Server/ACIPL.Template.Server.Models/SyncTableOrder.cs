using System;

namespace ACIPL.Template.Server.Models
{
    public class SyncTableOrder : BaseModel
    {
        public string Name { get; set; }
        public string TempTableName { get; set; }
        public string StoredProcedureName { get; set; }
        public int Order { get; set; }
        public bool ResponseData { get; set; }
        public string SyncMethod { get; set; }
        public DateTime ActionDate { get; set; }
        public string Action { get; set; }
    }
}
