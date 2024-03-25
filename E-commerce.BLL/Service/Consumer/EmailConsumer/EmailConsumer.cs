using E_commerce.Models;
using E_commerce.Models.DTO_s.Email;
using MailKit.Net.Smtp;
using MassTransit;
using MimeKit;

namespace E_commerce.BLL.Service.Consumer.EmailConsumer
{
    public class EmailConsumer : IConsumer<EmailMessageDTO>
    {
        private readonly EmailConfiguration _emailConfig;
        public EmailConsumer(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        /// <summary>
        /// Handles the processing of a EmailSendRequest message recieved by the message broker (RabbitMQ).
        /// With the use of MimeKit and SMTP server.
        /// </summary>
        /// <param name="context">The ConsumeContext contains a EmailSendRequest message</param>
        /// <returns>A Task representing the asynchronus operation.</returns>
        public Task Consume(ConsumeContext<EmailMessageDTO> context)
        {
            var EmailMessage = new EmailMessage(context.Message.To, context.Message.Subject, context.Message.Content);

            var message = CreateEmailMessage(EmailMessage);
            Send(message);

            return Task.CompletedTask;
        }

        private MimeMessage CreateEmailMessage(EmailMessage message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };

            return emailMessage;
        }

        private void Send(MimeMessage message)
        {
            using var client = new SmtpClient();

            client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
            client.AuthenticationMechanisms.Remove("X0AUTH2");
            client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

            client.Send(message);
        }
    }
}
