using System.Net;
using System.Web;
using newServer.Configuration;
using newServer.Services;

namespace newServer.Handler;

public class StaticFileHandler : IHandler
{
    private ServerConfiguration configuration;
    
    public StaticFileHandler(ServerConfiguration configuration)
    {
        this.configuration = configuration;
    }
    private static readonly List<string> typeList = new List<string>{            
        ".html", ".css", ".jpg" , ".svg", ".png", ".ico"
    };

    public async void Handler(HttpListenerContext context)
    {
        if (typeList.Contains(Path.GetExtension(context.Request.RawUrl!.Split("/").Last())) || context.Request.RawUrl == "/" )
            await RequestHandler(context);
        else
            new ControllHander(configuration).Handler(context);
    }

    public async Task RequestHandler(HttpListenerContext context)
    {
        HttpListenerRequest request = context.Request;
        HttpListenerResponse response = context.Response;
        string absolutePath = request.Url!.AbsolutePath;
        var filePath = absolutePath;
        if (filePath == "/")
        {
            filePath = configuration.StaticFilesPath + "/" + "index.html";
        }
            
        if (filePath.StartsWith("/"))
            filePath = filePath.Trim('/');
        
        
        if (!File.Exists(filePath))
        {
            if (!File.Exists(configuration!.StaticFilesPath + "/" + filePath))
            {
                filePath = $"{configuration.StaticFilesPath}/404.html";
            }
            else
            {
                filePath = configuration.StaticFilesPath + "/" + filePath;
            }
        }

        string extension = Path.GetExtension(filePath);

        ParseExtention(extension, response);

        using (var fileStream = File.OpenRead(filePath))
        {
            await fileStream.CopyToAsync(response.OutputStream);
        }

        response.Close();
    }
    private void ParseExtention(string extention, HttpListenerResponse response)
    {
        switch (extention)
        {
            case ".html":
                response.ContentType = "text/html";
                break;
          
            case ".css":
                response.ContentType = "text/css";
                break;
            case ".jpeg":
                response.ContentType = "image/jpeg";
                break;
            case ".png":
                response.ContentType = "image/png";
                break;
            case ".svg":
                response.ContentType = "image/svg+xml";
                break;
            case ".ico":
                response.ContentType = "image/x-icon";
                break;
        }
    }
}
