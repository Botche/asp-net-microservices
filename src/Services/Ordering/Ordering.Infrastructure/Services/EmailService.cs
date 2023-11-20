namespace Ordering.Infrastructure.Mail
{
    using System.Net;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using Ordering.Application.Contracts.Infrastructure;
    using Ordering.Application.Models;

    using SendGrid;
    using SendGrid.Helpers.Mail;

    public class EmailService : IEmailService
    {
        private readonly EmailSettings emailSettings;
        private readonly ILogger<EmailService> logger;

        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            this.emailSettings = emailSettings.Value;
            this.logger = logger;
        }

        public async Task<bool> SendEmail(Email email)
        {
            SendGridClient client = new(this.emailSettings.ApiKey);

            string subject = email.Subject;
            EmailAddress to = new(email.To);
            string emailBody = email.Body;

            EmailAddress from = new()
            {
                Email = this.emailSettings.FromAddress,
                Name = this.emailSettings.FromName,
            };

            SendGridMessage sendGridMessage = MailHelper.CreateSingleEmail(from, to, subject, emailBody, emailBody);
            Response response = await client.SendEmailAsync(sendGridMessage);

            if (response.StatusCode != HttpStatusCode.Accepted
                && response.StatusCode != HttpStatusCode.OK)
            {
                this.logger.LogError("Email sending failed.");

                return false;
            }

            this.logger.LogInformation("Email sent.");

            return true;
        }
    }
}
