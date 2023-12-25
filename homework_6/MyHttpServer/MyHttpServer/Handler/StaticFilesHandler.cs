using System.Net;
using System.Web;
using MyHttpServer.Configuration;
using MyHttpServer.Services;

namespace MyHttpServer.Handler
{
    public class StaticFilesHandler : IHandler
    {
        private readonly ServerConfiguration _configuration;

        public StaticFilesHandler(ServerConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async void Handle(HttpListenerContext context)
        {
            try
            {
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;
                string absolutePath = request.Url!.AbsolutePath, filePath = absolutePath;

                if (request.HttpMethod.Equals("Get", StringComparison.OrdinalIgnoreCase))
                {
                    if (filePath == "/")
                        filePath = _configuration!.StaticFilesPath + "/" + "index.html";

                    if (filePath.StartsWith("/"))
                        filePath = filePath.Trim('/');

                    if (!File.Exists(filePath))
                    {
                        if (!File.Exists(_configuration!.StaticFilesPath + "/" + filePath))
                        {
                            response.StatusCode = (int)HttpStatusCode.NotFound;
                            filePath = $"{_configuration.StaticFilesPath}/404.html";
                        }
                        else
                        {
                            filePath = _configuration.StaticFilesPath + "/" + filePath;
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
                else if (request.HttpMethod.Equals("Post", StringComparison.OrdinalIgnoreCase))
                {
                    string str = await new StreamReader(request.InputStream).ReadToEndAsync();
                    EmailSender sender = new EmailSender(_configuration);
                    sender.SendEmail(HttpUtility.UrlDecode(str.Split('&')[0].Split('=')[1]), HttpUtility.UrlDecode(str.Split('&')[1].Split('=')[1]));
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error handling request: {ex.Message}");
                Console.ResetColor();
            }
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
}

