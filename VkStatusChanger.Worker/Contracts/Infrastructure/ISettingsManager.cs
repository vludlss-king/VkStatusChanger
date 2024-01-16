using VkStatusChanger.Worker.Models.UserSettings;

namespace VkStatusChanger.Worker.Contracts.Infrastructure
{
    public interface ISettingsManager
    {
        /// <summary>
        /// Записать настройки
        /// </summary>
        Task Write(Settings settings);
        /// <summary>
        /// Прочитать настройки
        /// </summary>
        Task<Settings> Read();
        /// <summary>
        /// Сбросить настройки
        /// </summary>
        void Reset();
    }
}
