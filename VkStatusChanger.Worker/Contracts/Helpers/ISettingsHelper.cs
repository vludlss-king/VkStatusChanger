using VkStatusChanger.Worker.Models.Settings;

namespace VkStatusChanger.Worker.Contracts.Helpers
{
    public interface ISettingsHelper
    {
        /// <summary>
        /// Записать настройки
        /// </summary>
        Task Write(SettingsModel settings);
        /// <summary>
        /// Прочитать настройки
        /// </summary>
        Task<SettingsModel> Read();
        /// <summary>
        /// Сбросить настройки
        /// </summary>
        void Reset();
    }
}
