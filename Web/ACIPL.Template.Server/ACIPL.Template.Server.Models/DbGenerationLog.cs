namespace ACIPL.Template.Server.Models
{
    public class DbGenerationLog : BaseModel
    {
        public string Token { get; set; }
        public ProcessStatusMaster ProcessStatusMasterId { get; set; }
        public string Message { get; set; }
    }
}
