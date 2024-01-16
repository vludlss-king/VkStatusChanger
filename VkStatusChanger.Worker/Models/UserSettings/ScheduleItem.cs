namespace VkStatusChanger.Worker.Models.UserSettings
{
    public class ScheduleItem
    {
        /// <summary>
        /// Текст статуса
        /// </summary>
        public string? StatusText { get; set; }
        /// <summary>
        /// Дата, в которую установить статус
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Время, в которое установить статус
        /// </summary>
        public TimeSpan Time { get; set; }
    }
}
