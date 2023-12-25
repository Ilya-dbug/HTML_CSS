namespace MyHttpServer.Attributes
{
    /// <summary>
    /// Класс атрбутов для контроллеров
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ControllerAttribute : Attribute
    {
        /// <summary>
        /// Тип контроллера
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Конструктор для определения типа контроллера
        /// </summary>
        /// <param name="type">Тип контроллера</param>
        public ControllerAttribute(string type) => Type = type;
    }
}

