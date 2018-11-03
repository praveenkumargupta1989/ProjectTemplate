using System.Collections.Generic;
using System.Net.Mail;

namespace ACIPL.Template.Core.Utilities
{
    public class MailDto
    {
        public MailDto()
        {
            To = new List<string>();
            CC = new List<string>();
            BCC = new List<string>();
            Attachments = new List<Attachment>();
            EnableSsl = false;
        }
        public string From { get; set; }
        public List<string> To { get; set; }
        public List<string> CC { get; set; }
        public List<string> BCC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<Attachment> Attachments { get; set; }
        public bool EnableSsl { get; set; }
    }
}
