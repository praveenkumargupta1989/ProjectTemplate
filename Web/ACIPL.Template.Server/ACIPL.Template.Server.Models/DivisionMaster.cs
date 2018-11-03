using System;

namespace ACIPL.Template.Server.Models
{
    public class DivisionMaster : BaseModel
    {
        public int CompanyMasterId { get; set; }

        public string Name { get; set; }

        public string Abbreviation { get; set; }

        public DateTime EditedDate { get; set; }

        public int EditedByEmp { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedByEmp { get; set; }


    }
}
