using Newtonsoft.Json;
using VkStatusChanger.Worker.Contracts.Helpers;
using VkStatusChanger.Worker.Models.Settings;

namespace VkStatusChanger.Worker.Helpers
{
    internal class SettingsHelper : ISettingsHelper
    {
        const string settingsFileName = "settings.json";

        private readonly ISettingsGet _settingsGet;

        public SettingsHelper(ISettingsGet settingsGet)
        {
            _settingsGet = settingsGet;

            if (!File.Exists(settingsFileName))
                File.Create(settingsFileName).Dispose();
        }

        public async Task WriteSettings(SettingsModel settings)
        {
            await File.WriteAllTextAsync(_settingsGet.SettingsFile, JsonConvert.SerializeObject(settings));
        }

        public async Task<SettingsModel> ReadSettings()
        {
            string settingsJson = await File.ReadAllTextAsync(_settingsGet.SettingsFile);
            SettingsModel settingsModel = JsonConvert.DeserializeObject<SettingsModel>(settingsJson) ?? new SettingsModel();

            return settingsModel;
        }
    }
}
