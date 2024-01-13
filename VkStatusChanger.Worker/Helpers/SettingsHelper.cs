using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using VkStatusChanger.Worker.Contracts.Helpers;
using VkStatusChanger.Worker.Models;
using VkStatusChanger.Worker.Models.Settings;

namespace VkStatusChanger.Worker.Helpers
{
    internal class SettingsHelper : ISettingsHelper
    {
        private readonly SettingsFile _settingsFile;

        public SettingsHelper(IOptions<SettingsFile> settingsFile)
        {
            _settingsFile = settingsFile.Value;

            if (!File.Exists(_settingsFile.FileName))
                File.Create(_settingsFile.FileName!).Dispose();
        }

        public async Task WriteSettings(SettingsModel settings)
        {
            await File.WriteAllTextAsync(_settingsFile.FileName!, JsonConvert.SerializeObject(settings));
        }

        public async Task<SettingsModel> ReadSettings()
        {
            string settingsJson = await File.ReadAllTextAsync(_settingsFile.FileName!);
            SettingsModel settingsModel = JsonConvert.DeserializeObject<SettingsModel>(settingsJson) ?? new SettingsModel();

            return settingsModel;
        }
    }
}
