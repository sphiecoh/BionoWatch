using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NetR.Web.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace NetR.Web.Notification
{
    public class EmailNotifier : INotifier
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<EmailNotifier> logger;

        public EmailNotifier(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            this.configuration = configuration;
            this.logger = loggerFactory.CreateLogger<EmailNotifier>();
        }
        public async Task Notify(NotificationModel notification)
        {
            if (!notification.IsEmail) return;
            var message = notification.Email.Value;
            var client = new SendGridClient(configuration["Email:SendGridKey"]);
            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress(message.FromAddress));
            msg.SetSubject(message.Subject);
            foreach (var item in message.Recipients.Split(';'))
            {
                msg.AddTo(new EmailAddress(item));
            }
            msg.HtmlContent = message.Message;
            var response = await client.SendEmailAsync(msg);
            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                logger.LogInformation("Email {subject} sent {to}",  message.Subject, string.Join(" ", message.Recipients));
                
            }
            else
            {

                foreach (var err in await response.DeserializeResponseBodyAsync(response.Body))
                {
                    logger.LogError("Failed - Email  {subject} could not be sent - {err}", message.Subject, err);
                }

            }
        }
    }
}
