namespace MyHttpServer.Attributes
{
    /// <summary>
    /// Класс атрибутов для HTTP методов типа Get
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class GetAttribute: Attribute, IHttpMethodAttribute
    {
        public GetAttribute(string actionName)
        {
            ActionName = actionName;
        }

        public string ActionName { get; }
    }
}

