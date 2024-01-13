namespace VkStatusChanger.Worker.Models.Settings
{
    internal class EveryModel
    {
        /// <summary>
        /// Тексты статусов
        /// </summary>
        public List<string>? StatusesTexts { get; set; } = new();
        /// <summary>
        /// Через сколько секунд менять статус
        /// </summary>
        public int Seconds { get; set; }
    }
}
