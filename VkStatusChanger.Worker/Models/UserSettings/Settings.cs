using VkStatusChanger.Worker.Enums;

namespace VkStatusChanger.Worker.Models.UserSettings
{
    public class Settings
    {
        /// <summary>
        /// Токен для входа в клиент VK API
        /// </summary>
        public string? AccessToken { get; set; }
        /// <summary>
        /// Один из типов настроек
        /// </summary>
        public Every Every { get; set; } = new();
        /// <summary>
        /// Один из типов настроек
        /// </summary>
        public Schedule Schedule { get; set; } = new();
        /// <summary>
        /// Тип настроек
        /// </summary>
        public SettingsType Type { get; set; }
    }
}
