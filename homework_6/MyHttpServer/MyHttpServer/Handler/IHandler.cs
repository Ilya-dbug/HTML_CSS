using System.Net;

namespace MyHttpServer.Handler
{
    public interface IHandler
    {
        public async void Handle(HttpListenerContext context) { }
    }
}
