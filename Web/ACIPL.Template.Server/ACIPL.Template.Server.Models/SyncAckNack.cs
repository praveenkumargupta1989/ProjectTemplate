namespace ACIPL.Template.Server.Models
{
    public class SyncAckNack : BaseModel
    {
        public string Name { get; set; }

        public string Status { get; set; }

        public string AckNack { get; set; }
    }
}
