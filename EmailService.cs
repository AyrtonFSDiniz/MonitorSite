using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    public class EnvioEmailService
    {

        public class SmtpConfiguration
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string Host { get; set; }
            public int Port { get; set; }
            public bool Ssl { get; set; }
        }
        public class EmailMessage
        {
            public string ToEmail { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }
            public bool IsHtml { get; set; }

            public object Attachment { get; set; }

        }
        public class EmailService
        {
            private readonly SmtpConfiguration _config;
            public EmailService()
            {
                _config = new SmtpConfiguration();
                var outlookUserName = "erozesteticaautomotiva@gmail.com";
                var outlookPassword = "";
                var outlookHost = "smtp.gmail.com";
                var outlookPort = 587;
                var outlookSsl = true;
                _config.Username = outlookUserName;
                _config.Password = outlookPassword;
                _config.Host = outlookHost;
                _config.Port = outlookPort;
                _config.Ssl = outlookSsl;

            }
            public bool SendEmailMessage(EmailMessage message)
            {
                var success = false;
                try
                {
                    var smtp = new SmtpClient
                    {
                        Host = _config.Host,
                        Port = _config.Port,
                        EnableSsl = _config.Ssl,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(_config.Username, _config.Password)
                    };
                    using (var smtpMessage = new MailMessage(_config.Username, message.ToEmail))
                    {
                        smtpMessage.Subject = message.Subject;
                        smtpMessage.Body = message.Body;
                        smtpMessage.IsBodyHtml = message.IsHtml;
                        smtpMessage.BodyEncoding = System.Text.Encoding.UTF8;
                        smtpMessage.Attachments.Add((Attachment)message.Attachment);
                        smtp.Send(smtpMessage);

                    }
                    success = true;
                }
                catch (Exception ex)
                {
                    //todo: add logging integration
                    throw new Exception(ex.Message);
                }
                return success;
            }
        }
    }
}
