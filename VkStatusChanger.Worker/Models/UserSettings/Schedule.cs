namespace VkStatusChanger.Worker.Models.UserSettings
{
    public class Schedule
    {
        /// <summary>
        /// Расписания смены статусов
        /// </summary>
        public List<ScheduleItem> Items { get; set; } = new();
    }
}
