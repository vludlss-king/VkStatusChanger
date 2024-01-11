namespace VkStatusChanger.Worker.Models.Settings
{
    internal class ScheduleItem
    {
        public string? StatusText { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
    }
}
