using System.Net;
using System.Web;
using System.Reflection;
using MyHttpServer.Configuration;
using MyHttpServer.Attributes;
using System.Text;

namespace MyHttpServer.Handler
{
    public class ControllersHandler: IHandler
    {
        private ServerConfiguration _configuration;
        public ControllersHandler(ServerConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async void Handle(HttpListenerContext context)
        {
            try
            {
                var strParams = context?.Request.Url!
                    .Segments
                    .Skip(1)
                    .Select(s => s.Replace("/", ""))
                    .ToArray();

                if (strParams!.Length >= 2)
                {
                    string input = await new StreamReader(context.Request!.InputStream).ReadToEndAsync();

                    (string, string) user = ("", "");
                    if (!String.IsNullOrEmpty(input))
                    {
                        user = (HttpUtility.UrlDecode(input.Split('&')[0].Split('=')[1]), HttpUtility.UrlDecode(input.Split('&')[1].Split('=')[1]));
                    }

                    string controllerName = strParams[0], methodName = strParams[1];
                    var assembly = Assembly.GetExecutingAssembly();

                    var controller = assembly.GetTypes()
                        .Where(t => Attribute.IsDefined(t, typeof(ControllerAttribute)))
                        .FirstOrDefault(c => ((ControllerAttribute)Attribute.GetCustomAttribute(c, typeof(ControllerAttribute))!)
                        .Type.Equals(controllerName, StringComparison.OrdinalIgnoreCase));

                    var method = controller?.GetMethods()
                       .Where(x => x.GetCustomAttributes(true)
                       .Any(attr => attr.GetType().Name.Equals($"{context.Request.HttpMethod}Attribute", StringComparison.OrdinalIgnoreCase)))
                       .FirstOrDefault(m => m.Name.Equals(methodName, StringComparison.OrdinalIgnoreCase));

                    var queryParams = new object[] { };

                    if (user != ("", ""))
                    {
                        queryParams = new object[] { user.Item1, user.Item2, _configuration};
                        context.Response.Redirect("/");
                    }

                    var result = method?.Invoke(Activator.CreateInstance(controller), queryParams);
                }
                else
                {
                    Console.WriteLine("Another handler");
                    throw new ArgumentException("Failed to process request!");
                }
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Controller handler: " + ex.Message);
                Console.ResetColor();
            }
        }
    }
}

