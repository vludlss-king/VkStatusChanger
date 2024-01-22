namespace VkStatusChanger.Worker.Models.UserSettings
{
    public class DateTimeScheduleItem
    {
        /// <summary>
        /// Статус
        /// </summary>
        public string? Status { get; set; }
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
