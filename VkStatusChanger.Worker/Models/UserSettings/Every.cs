namespace VkStatusChanger.Worker.Models.UserSettings
{
    public class Every
    {
        /// <summary>
        /// Тексты статусов
        /// </summary>
        public List<string> StatusesTexts { get; set; } = new();
        /// <summary>
        /// Через сколько секунд менять статус
        /// </summary>
        public int Seconds { get; set; }
    }
}
