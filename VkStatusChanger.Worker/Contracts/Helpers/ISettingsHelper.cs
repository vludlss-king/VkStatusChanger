using VkStatusChanger.Worker.Models.Settings;

namespace VkStatusChanger.Worker.Contracts.Helpers
{
    public interface ISettingsHelper
    {
        Task WriteSettings(SettingsModel settings);
        Task<SettingsModel> ReadSettings();
        void ResetSettings();
    }
}
