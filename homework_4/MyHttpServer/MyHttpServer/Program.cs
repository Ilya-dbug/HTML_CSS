namespace MyHttpServer
{
    class Program
    {
        public static async Task Main()
        {
            Server server = new Server();
            await server.Start();
        }
    }
}