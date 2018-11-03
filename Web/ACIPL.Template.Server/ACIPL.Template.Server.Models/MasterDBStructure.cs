using System;

namespace ACIPL.Template.Server.Models
{
    public class MasterDBStructure : BaseModel
    {
        public int Result { get; set; }

        public string TableName { get; set; }

        public DateTime EditedDate { get; set; }
    }
}
