using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace TimberV2.Models
{
    public class EmailSender
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public void sendEmail()
        {
            MailMessage msg = new MailMessage(System.Configuration.ConfigurationManager.AppSettings["Email"].ToString(), To);
            msg.Subject = Subject;
            msg.Body = Body;
            msg.IsBodyHtml = false;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Timeout = 10000000;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            NetworkCredential networkCredential = new NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["Email"].ToString(), System.Configuration.ConfigurationManager.AppSettings["appPass"].ToString());
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = networkCredential;
            smtp.Send(msg);
        }
    }
}