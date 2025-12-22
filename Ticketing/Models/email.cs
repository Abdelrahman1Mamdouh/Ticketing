using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Web;

namespace Ticketing
{
    public class email
    {
        public string to { get; set; }
        public string from { get; set; }
        public string subject { get; set; }
        public string body { get; set; }


        public static void sendMail(string to, string from, string subject, string body)
        {
                   
       
            MailMessage mail = new MailMessage(to, from, subject, body);
            SmtpClient SmtpServer = new SmtpClient("smtp.your-isp.com");
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("username", "password");
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail);

        }
       

    }
}