using System;

namespace ACIPL.Template.Server.Models
{
    public class BaseModel
    {
        public DateTime CreatedDate { get; set; }
        public int Id { get; set; }
        public bool Active { get; set; }
        public int EmployeeId { get; set; }
        public DateTime EditedDate { get; set; }
    }
}
