namespace VkStatusChanger.Worker.Models.UserSettings
{
    public class DateTimeSchedule
    {
        /// <summary>
        /// Расписания смены статусов
        /// </summary>
        public List<DateTimeScheduleItem> Items { get; set; } = new();
    }
}
