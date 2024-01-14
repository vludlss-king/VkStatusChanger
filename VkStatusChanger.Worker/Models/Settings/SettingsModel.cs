using VkStatusChanger.Worker.Enums;

namespace VkStatusChanger.Worker.Models.Settings
{
    public class SettingsModel
    {
        /// <summary>
        /// Токен для входа в клиент VK API
        /// </summary>
        public string? AccessToken { get; set; }
        /// <summary>
        /// Один из типов настроек
        /// </summary>
        public EveryModel? Every { get; set; } = new();
        /// <summary>
        /// Один из типов настроек
        /// </summary>
        public ScheduleModel? Schedule { get; set; } = new();
        /// <summary>
        /// Тип настроек
        /// </summary>
        public SettingsType SettingsType { get; set; }
    }
}
