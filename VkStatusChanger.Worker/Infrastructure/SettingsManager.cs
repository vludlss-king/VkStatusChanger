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
                File.Create(_settingsFile.Name).Dispose();
        }

        public async Task Write(UserSettingsModel settings)
        {
            await File.WriteAllTextAsync(_settingsFile.Name, JsonConvert.SerializeObject(settings));
        }

        public async Task<UserSettingsModel> Read()
        {
            string settingsJson = await File.ReadAllTextAsync(_settingsFile.Name);
            UserSettingsModel settingsModel = JsonConvert.DeserializeObject<UserSettingsModel>(settingsJson) ?? new UserSettingsModel();

            return settingsModel;
        }

        public void Reset()
        {
            if (File.Exists(_settingsFile.Name))
            {
                File.Delete(_settingsFile.Name);
                File.Create(_settingsFile.Name).Dispose();
            }
        }
    }
}