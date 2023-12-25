using MimeKit;
using MailKit.Net.Smtp;
using MyHttpServer.Configuration;
using MyHttpServer.Services;

namespace MyHttpServer.Services
{
    public class EmailSender : IEmailSenderService
    {
        private static ServerConfiguration? _configuration;

        public EmailSender(ServerConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmail(string email, string password)
        {
            try
            {
                using var smtpClient = new SmtpClient();
                MimeMessage message = GenerateMessage(email, password);

                smtpClient.Connect(_configuration.SmtpServerHost, _configuration.SmtpServerPort, true);
                smtpClient.Authenticate(_configuration.MailSender, _configuration.PasswordSender);
                smtpClient.Send(message);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("==== Message sent! ====\n");
                Console.ResetColor();
                smtpClient.Disconnect(true);

            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Failed to send mail message: {ex.Message}");
                Console.ResetColor();
            }
        }

        public MimeMessage GenerateMessage(string email, string password)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("HTTP Server", _configuration.MailSender));
            message.To.Add(new MailboxAddress("", _configuration.FromEmail));
            message.Subject = "Hello from battle.net!";
            var builder = new BodyBuilder();

            builder.HtmlBody = string.Format
            (
                $"<body style=\"font-family: Arial, sans-serif; background-color: #ffffff; " +
                $"margin: 0; padding: 0; display: flex; justify-content: center; align-items: center; " +
                $"min-height: 100vh;\"><div style=\"background-color: #15171E; padding: 20px; " +
                $"border-radius: 10px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.2); height: 300px; width: 500px; " +
                $"text-align: center; color: #ffffff; display: flex; flex-direction: column; justify-content: center;\">" +
                $"<h1 style=\"font-size: 30px;\">Your Account Information</h1>" +
                $"<h4 style=\"font-size: 24px; font-style: bold;\">Email: {email}</h4><h4 style=\"font-size: 24px;font-style: bold;\">" +
                $"Password: {password}</h4></div></body>"
            );

            message.Body = builder.ToMessageBody();

            return message;
        }
    }
}