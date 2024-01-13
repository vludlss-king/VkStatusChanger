using VkStatusChanger.Worker.Contracts.Helpers;
using VkStatusChanger.Worker.Models;
using VkStatusChanger.Worker.Models.Settings;

namespace VkStatusChanger.Worker.Controllers
{
    internal class SettingsCommandController
    {
        private readonly ISettingsHelper _settingsHelper;

        public SettingsCommandController(ISettingsHelper settingsHelper)
        {
            _settingsHelper = settingsHelper;
        }

        public async Task EverySet(SettingsCommand.EveryCommand.SetCommand command)
        {
            var settings = await _settingsHelper.ReadSettings();

            settings.Every!.StatusesTexts = command.StatusesTexts.ToList();
            settings.Every.Seconds = command.Seconds;

            await _settingsHelper.WriteSettings(settings);
        }

        public async Task EveryShow(SettingsCommand.EveryCommand.ShowCommand command)
        {
            var settings = await _settingsHelper.ReadSettings();

            Console.WriteLine($"Менять каждые {settings.Every.Seconds} секунд, Статусы:");

            if (!settings.Every.StatusesTexts!.Any())
            {
                Console.WriteLine("отсутствуют");
            }
            else
            {
                for(int index = 0; index < settings.Every.StatusesTexts.Count; index++)
                {
                    var statusText = settings.Every.StatusesTexts[index];
                    Console.WriteLine($"{index + 1}. {statusText}");
                }
            }
        }

        public async Task ScheduleAdd(SettingsCommand.ScheduleCommand.AddCommand command)
        {
            var settings = await _settingsHelper.ReadSettings();

            var scheduleItem = new ScheduleItem
            {
                StatusText = command.StatusText,
                Date = command.Date,
                Time = command.Time,
            };
            settings.Schedule!.Items!.Add(scheduleItem);

            await _settingsHelper.WriteSettings(settings);
        }

        public async Task ScheduleEdit(SettingsCommand.ScheduleCommand.EditCommand command)
        {
            var settings = await _settingsHelper.ReadSettings();

            var item = settings.Schedule.Items[command.Id - 1];

            item.StatusText = command.StatusText;
            item.Date = command.Date;
            item.Time = command.Time;

            await _settingsHelper.WriteSettings(settings);
        }

        public async Task ScheduleRemove(SettingsCommand.ScheduleCommand.RemoveCommand command)
        {
            var settings = await _settingsHelper.ReadSettings();

            if (command.Id.HasValue)
                settings.Schedule!.Items!.RemoveAt(command.Id.Value - 1);
            else
                settings.Schedule!.Items!.RemoveAll(_ => true);

            await _settingsHelper.WriteSettings(settings);
        }

        public async Task ScheduleList(SettingsCommand.ScheduleCommand.ListCommand command)
        {
            var settings = await _settingsHelper.ReadSettings();
            
            for(int index = 0; index < settings.Schedule!.Items!.Count; index++)
            {
                var scheduleItem = settings.Schedule.Items[index];
                Console.WriteLine($"{index + 1}. Статус: {scheduleItem.StatusText}, Дата: {scheduleItem.Date:dd.MM.yyyy}, Время: {scheduleItem.Time}");
            }
        }
    }
}
