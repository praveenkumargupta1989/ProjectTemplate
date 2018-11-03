using System;
using System.Net.Mail;

namespace ACIPL.Template.Core.Utilities
{
    public interface IEmailManager
    {
        void Send(MailDto mail);
    }

    public class EmailManager : IEmailManager
    {
        private readonly string hostAddress, userName, password, toAddresses, ccAddresses, bccAddresses, displayName;
        private readonly int portNum;

        public EmailManager(IConfigurationManager configurationManager)
        {
            hostAddress = configurationManager.GetDecryptedConfigurationValue("HostAddress");
            portNum = Convert.ToInt32(configurationManager.GetDecryptedConfigurationValue("PortNum"));
            userName = configurationManager.GetDecryptedConfigurationValue("EmailUserName");
            password = configurationManager.GetDecryptedConfigurationValue("EmailPassword");
            toAddresses = configurationManager.GetDecryptedConfigurationValue("ToEmailAddresses");
            ccAddresses = configurationManager.GetDecryptedConfigurationValue("CCEmailAddresses");
            bccAddresses = configurationManager.GetDecryptedConfigurationValue("BCCEmailAddresses");
            displayName = configurationManager.GetConfigurationValue("EmailDisplayName");
        }

        public void Send(MailDto mail)
        {
            var mailMessage = new MailMessage();

            if (mail.To.Count == 0)
            {
                mail.To.Add(toAddresses);
            }

            if (mail.CC.Count == 0)
            {
                mail.CC.Add(ccAddresses);
            }

            if (mail.BCC.Count == 0)
            {
                mail.BCC.Add(bccAddresses);
            }

            foreach (var emailId in mail.BCC)
            {
                if (!string.IsNullOrEmpty(emailId) && emailId.Trim() == "") continue;

                var email = emailId.Split(',');
                foreach (var item in email)
                {
                    mailMessage.Bcc.Add(new MailAddress(item));
                }
            }

            foreach (var emailId in mail.CC)
            {
                if (!string.IsNullOrEmpty(emailId) && emailId.Trim() == "") continue;

                var email = emailId.Split(',');
                foreach (var item in email)
                {
                    mailMessage.CC.Add(new MailAddress(item));
                }
            }

            foreach (var emailId in mail.To)
            {
                if (!string.IsNullOrEmpty(emailId) && emailId.Trim() == "") continue;

                var email = emailId.Split(',');
                foreach (var item in email)
                {
                    mailMessage.To.Add(new MailAddress(item));
                }
            }

            mailMessage.From = mail.From == null ? new MailAddress(userName) : new MailAddress(mail.From);
            mailMessage.Subject = mail.Subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = mail.Body;
            mailMessage.From = new MailAddress(userName);
            foreach (var file in mail.Attachments)
            {
                mailMessage.Attachments.Add(file);
            }

            var smtp = new SmtpClient
            {
                Host = hostAddress,
                EnableSsl = mail.EnableSsl,
                UseDefaultCredentials = true,
                Credentials = new System.Net.NetworkCredential(userName, password),
                Port = portNum
            };
            smtp.Send(mailMessage);
        }
    }
}
