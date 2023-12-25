using MyHttpServer.Services;
using MyHttpServer.Configuration;
using MyHttpServer.Attributes;

namespace MyHttpServer.Controllers
{
    /// <summary>
    /// Контроллер для аутентификации и отправки сообщений
    /// </summary>
    [Controller("Authentication")]
    public class AuthenticationController
    {
        [Post("SendEmail")]
        public void SendEmail(string email, string password, ServerConfiguration configuration)
        {
            EmailSender sender = new EmailSender(configuration);
            sender.SendEmail(email, password);
        }
    }
}

