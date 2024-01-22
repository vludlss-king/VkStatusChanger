using VkStatusChanger.Worker.Enums;

namespace VkStatusChanger.Worker.Models.UserSettings
{
    public class UserSettingsModel
    {
        /// <summary>
        /// Токен для входа в клиент VK API
        /// </summary>
        public string? AccessToken { get; set; }
        /// <summary>
        /// Один из типов настроек
        /// </summary>
        public EverySecondsSchedule EverySecondsSchedule { get; set; } = new();
        /// <summary>
        /// Один из типов настроек
        /// </summary>
        public DateTimeSchedule DateTimeSchedule { get; set; } = new();
        /// <summary>
        /// Тип настроек
        /// </summary>
        public SettingsType Type { get; set; }
    }
}
