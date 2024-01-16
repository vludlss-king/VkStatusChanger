using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using VkStatusChanger.Worker.Contracts.Infrastructure;
using VkStatusChanger.Worker.Models;
using VkStatusChanger.Worker.Models.UserSettings;

namespace VkStatusChanger.Worker.Infrastructure
{
    internal class SettingsManager : ISettingsManager
    {
        private readonly SettingsFile _settingsFile;

        public SettingsManager(IOptions<SettingsFile> settingsFile)
        {
            _settingsFile = settingsFile.Value;

            if (!File.Exists(_settingsFile.Name))
                File.Create(_settingsFile.Name!).Dispose();
        }

        public async Task Write(Settings settings)
        {
            await File.WriteAllTextAsync(_settingsFile.Name!, JsonConvert.SerializeObject(settings));
        }

        public async Task<Settings> Read()
        {
            string settingsJson = await File.ReadAllTextAsync(_settingsFile.Name!);
            Settings settingsModel = JsonConvert.DeserializeObject<Settings>(settingsJson) ?? new Settings();

            return settingsModel;
        }

        public void Reset()
        {
            if (File.Exists(_settingsFile.Name))
            {
                File.Delete(_settingsFile.Name);
                File.Create(_settingsFile.Name!).Dispose();
            }
        }
    }
}