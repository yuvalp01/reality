using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Nadlan
{

    public class Notifications
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        //private readonly string LOGIC_APP_URL;
        public Notifications(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            //LOGIC_APP_URL = _configuration.GetValue<string>("Email:LOGIC_APP_URL");
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task Send(string userName, string type)
        {
            try
            {
                bool useEmailNotification = _configuration.GetValue<bool>("Email:UseEmailNotification");
                if (useEmailNotification)
                {
                    string LOGIC_APP_URL = _configuration.GetValue<string>("Email:LOGIC_APP_URL");
                    string URL = _configuration.GetValue<string>("Email:URL");
                    string notificationUrl;
                    string body;
                    string assistentEmail = _configuration.GetValue<string>("Email:assistentEmail");
                    string managerEmail = _configuration.GetValue<string>("Email:managerEmail");
                    string sendTo = userName.ToLower() == "stella" ? managerEmail : assistentEmail;

                    string subject;
                    if (type=="issue")
                    {
                        subject = "A new issue has been created";
                        body = "A new issue has been created.";
                        notificationUrl = $"<a href='{URL}/issue-list'>here</a>.";
                    }
                    else
                    {
                        subject = "You have a new message";
                        body = "You have a new message";
                        notificationUrl = $"<a href='{URL}'>here</a>.";
                    }
                   
                    var jsonData = JsonSerializer.Serialize(new
                    {
                        email = sendTo,
                        content = body,
                        subject = subject,
                        url = notificationUrl

                    });

                    HttpResponseMessage result = await _httpClient.PostAsync(
                        LOGIC_APP_URL,
                        new StringContent(jsonData, Encoding.UTF8, "application/json"));
                    var statusCode = result.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                //TODO - log
            }
           
        }


        //public async Task Send()
        //{
        //    try
        //    {
        //        bool useEmailNotification = _configuration.GetValue<bool>("Email:UseEmailNotification");
        //        if (useEmailNotification)
        //        {
        //            string apiKey = _configuration.GetValue<string>("Email:SENDGRID_KEY");
        //            string sender = _configuration.GetValue<string>("Email:Sender");
        //            var from = new EmailAddress(sender, "Message Notification");
        //            EmailAddress to = new EmailAddress("yp1976@gmail.com", "The Other");

        //            var client = new SendGridClient(apiKey);

        //            var subject = $"A new issue has been created";
        //            // var to = new EmailAddress("test@example.com", "Example User");
        //            var plainTextContent = "There is a new issue in the system, please check.";
        //            // var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
        //            var htmlContent = $"There is a new issue in the system, please check <a href='https://nadlanapp.azurewebsites.net/issue-list'>here.</a> ";

        //            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        //            var response = await client.SendEmailAsync(msg);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //       // throw;
        //    }
        //}
        //public void SendEmail(string messageType)
        //{
        //    try
        //    {
        //        bool useEmailNotification = _configuration.GetValue<bool>("Email:UseEmailNotification");
        //        string host = _configuration.GetValue<string>("Email:Host");
        //        int port = _configuration.GetValue<int>("Email:Port");
        //        string sender = _configuration.GetValue<string>("Email:Sender");
        //        string userName = _configuration.GetValue<string>("Email:UserName");
        //        string password = _configuration.GetValue<string>("Email:Password");
        //        MailAddress receiver = new MailAddress("yp1976@gmail.com", "The Other");
        //        if (useEmailNotification)
        //        {
        //            MailAddress SENDER = new MailAddress("yuvalp01@gmail.com", "Message Notification");
        //            using (var email = new MailMessage())
        //            {
        //                email.IsBodyHtml = true;
        //                email.From = new MailAddress(sender, "Message Notification");
        //                email.To.Add(receiver);
        //                email.Subject = $"You have a new {messageType}";
        //                email.Body = $"You have a new {messageType}, please check <a href='http://nadlanapp.azurewebsites.net'>here.</a> ";
        //                using (SmtpClient client = new SmtpClient())
        //                {
        //                    client.EnableSsl = true;
        //                    client.UseDefaultCredentials = false;
        //                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //                    client.Host =host ;
        //                    client.Port = port;
        //                    //client.Credentials = new NetworkCredential("yuvalp01@gmail.com", "hvwtkbwagreznayt");
        //                    client.Credentials = new NetworkCredential(userName, password);
        //                    client.Send(email);
        //                }
        //            }
        //        }
        //    }

        //    catch (Exception ex)
        //    {

        //        // throw;
        //    }
        //}
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
