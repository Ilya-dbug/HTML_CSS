using System.Net;

namespace newServer.Handler;

public interface IHandler
{
    public async void Handler(HttpListenerContext context) { }
}