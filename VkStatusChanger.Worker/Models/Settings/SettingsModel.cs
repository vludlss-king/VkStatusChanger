using VkStatusChanger.Worker.Enums;

namespace VkStatusChanger.Worker.Models.Settings
{
    internal class SettingsModel
    {
        public string? AccessToken { get; set; }
        public EveryModel? Every { get; set; } = new();
        public ScheduleModel? Schedule { get; set; } = new();
        public SettingsType SettingsType { get; set; }
    }
}
