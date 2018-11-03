namespace ACIPL.Template.Server.Models
{
    public class NotificationStatus : BaseModel
    {
        public string Command { get; set; }
        public bool IsAck { get; set; }

    }

}
