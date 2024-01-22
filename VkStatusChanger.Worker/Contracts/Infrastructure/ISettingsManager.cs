using System.Diagnostics.CodeAnalysis;
using VkStatusChanger.Worker.Models.UserSettings;

namespace VkStatusChanger.Worker.Contracts.Infrastructure
{
    public interface ISettingsManager
    {
        /// <summary>
        /// Записать настройки
        /// </summary>
        Task Write(UserSettingsModel settings);
        /// <summary>
        /// Прочитать настройки
        /// </summary>
        [return: NotNull]
        Task<UserSettingsModel> Read();
        /// <summary>
        /// Сбросить настройки
        /// </summary>
        void Reset();
    }
}
