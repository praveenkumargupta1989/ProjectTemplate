namespace ACIPL.Template.Server.Models
{
    public class Login : BaseModel
    {
        public int EmpId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsValidUser { get; set; }
    }
}
