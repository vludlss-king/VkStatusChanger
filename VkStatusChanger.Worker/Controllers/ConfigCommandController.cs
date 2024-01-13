using VkStatusChanger.Worker.Contracts.Helpers;
using VkStatusChanger.Worker.Helpers;
using VkStatusChanger.Worker.Models;
using VkStatusChanger.Worker.Models.Settings;

namespace VkStatusChanger.Worker.Controllers
{
    internal class ConfigCommandController
    {
        private readonly ISettingsHelper _settingsHelper;

        public ConfigCommandController(ISettingsHelper settingsHelper)
        {
            _settingsHelper = settingsHelper;
        }

        public async Task EverySet(Config.Every.Set command)
        {
            var settings = await _settingsHelper.ReadSettings();

            settings.Every.StatusesTexts = command.StatusesTexts.ToList();
            settings.Every.Seconds = command.Seconds;

            await _settingsHelper.WriteSettings(settings);
        }

        public async Task<EveryModel?> EveryShow(Config.Every.Show command)
        {
            var settings = await _settingsHelper.ReadSettings();

            return settings.Every;
        }

        public async Task ScheduleAdd(Config.Schedule.Add command)
        {
            var settings = await _settingsHelper.ReadSettings();

            var scheduleItem = new ScheduleItem
            {
                StatusText = command.StatusText,
                Date = command.Date,
                Time = command.Time,
            };
            settings.Schedule.Items.Add(scheduleItem);

            await _settingsHelper.WriteSettings(settings);
        }

        public async Task ScheduleUpdate(Config.Schedule.Edit command)
        {
            var settings = await _settingsHelper.ReadSettings();

            var item = settings.Schedule.Items[command.Id - 1];

            item.StatusText = command.StatusText;
            item.Date = command.Date;
            item.Time = command.Time;

            await _settingsHelper.WriteSettings(settings);
        }

        public async Task ScheduleRemove(Config.Schedule.Remove command)
        {
            var settings = await _settingsHelper.ReadSettings();

            if (command.Id.HasValue)
                settings.Schedule.Items.RemoveAt(command.Id.Value - 1);
            else
                settings.Schedule.Items.RemoveAll(_ => true);

            await _settingsHelper.WriteSettings(settings);
        }

        public async Task<List<ScheduleItem>> ScheduleList(Config.Schedule.List command)
        {
            var settings = await _settingsHelper.ReadSettings();

            return settings.Schedule.Items;
        }
    }
}
