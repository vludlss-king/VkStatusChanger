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

            if (!File.Exists(_settingsFile.Name))
                File.Create(_settingsFile.Name!).Dispose();
        }

        public async Task WriteSettings(SettingsModel settings)
        {
            await File.WriteAllTextAsync(_settingsFile.Name!, JsonConvert.SerializeObject(settings));
        }

        public async Task<SettingsModel> ReadSettings()
        {
            string settingsJson = await File.ReadAllTextAsync(_settingsFile.Name!);
            SettingsModel settingsModel = JsonConvert.DeserializeObject<SettingsModel>(settingsJson) ?? new SettingsModel();

            return settingsModel;
        }

        public void ResetSettings()
        {
            if (File.Exists(_settingsFile.Name))
            {
                File.Delete(_settingsFile.Name);
                File.Create(_settingsFile.Name!).Dispose();
            }
        }
    }
}