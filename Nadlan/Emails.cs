using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;

namespace Nadlan
{
    public class Emails
    {
        private readonly IConfiguration _configuration;

        public Emails(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmail(string messageType)
        {
            try
            {
                bool useEmailNotification = _configuration.GetValue<bool>("Email:UseEmailNotification");
                string host = _configuration.GetValue<string>("Email:Host");
                int port = _configuration.GetValue<int>("Email:Port");
                string sender = _configuration.GetValue<string>("Email:Sender");
                string userName = _configuration.GetValue<string>("Email:UserName");
                string password = _configuration.GetValue<string>("Email:Password");
                MailAddress receiver = new MailAddress("yp1976@gmail.com", "The Other");
                if (useEmailNotification)
                {
                    MailAddress SENDER = new MailAddress("yuvalp01@gmail.com", "Message Notification");
                    using (var email = new MailMessage())
                    {
                        email.IsBodyHtml = true;
                        email.From = new MailAddress(sender, "Message Notification");
                        email.To.Add(receiver);
                        email.Subject = $"You have a new {messageType}";
                        email.Body = $"You have a new {messageType}, please check <a href='http://nadlanapp.azurewebsites.net'>here.</a> ";
                        using (SmtpClient client = new SmtpClient())
                        {
                            client.EnableSsl = true;
                            client.UseDefaultCredentials = false;
                            client.DeliveryMethod = SmtpDeliveryMethod.Network;
                            client.Host =host ;
                            client.Port = port;
                            //client.Credentials = new NetworkCredential("yuvalp01@gmail.com", "hvwtkbwagreznayt");
                            client.Credentials = new NetworkCredential(userName, password);
                            client.Send(email);
                        }
                    }
                }
            }

            catch (Exception ex)
            {

                // throw;
            }
        }
    }
}



    //public void SendEmail(Message message)
    //{
    //    MailAddress SENDER = new  MailAddress ("yuvalp01@gmail.com","Message Notification");
    //    using (var email = new MailMessage())
    //    {
    //        email.IsBodyHtml = true;
    //        email.From = SENDER;
    //        email.To.Add(new MailAddress("yp1976@gmail.com", "The Other"));
    //        email.Subject = $"You have a new message";
    //        email.Body = $"You have a new message, please check <a href='http://nadlanapp.azurewebsites.net'>here</a> ";
    //        using (SmtpClient client = new SmtpClient())
    //        {
    //            client.EnableSsl = true;
    //            client.UseDefaultCredentials = false;
    //            client.DeliveryMethod = SmtpDeliveryMethod.Network;
    //            client.Host = "smtp.gmail.com"; ;
    //            client.Port = 587;
    //            //client.Credentials = new NetworkCredential("yuvalp01@gmail.com", "hvwtkbwagreznayt");
    //            client.Credentials = new NetworkCredential("yuvalp01@gmail.com", "hvwtkbwagreznayt");
    //            client.Send(email);
    //        }
    //    }

//    //}
//}
//}
