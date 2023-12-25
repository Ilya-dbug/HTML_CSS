namespace MyHttpServer.Attributes
{
    /// <summary>
    /// Интерфейс для определения типа HTTP метода
    /// </summary>
    public interface IHttpMethodAttribute
    {
        public string ActionName { get; }
    }
}

