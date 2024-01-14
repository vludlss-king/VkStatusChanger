using CommandLine;
using VkStatusChanger.Worker.Contracts.Helpers;
using VkStatusChanger.Worker.Contracts.Infrastructure;
using VkStatusChanger.Worker.Controllers.Common;
using VkStatusChanger.Worker.Models.Commands;
using VkStatusChanger.Worker.Models.Settings;

namespace VkStatusChanger.Worker.Controllers
{
    internal class SettingsCommandController : BaseController
    {
        private readonly ISettingsHelper _settingsHelper;

        public SettingsCommandController(ICustomParserResult parserResult, ISettingsHelper settingsHelper)
            : base(parserResult)
        {
            _settingsHelper = settingsHelper;
        }

        public override async Task ExecuteCommand()
        {
            MapCommand<SettingsCommand.ResetCommand>(Reset);
            await MapCommandAsync<SettingsCommand.TypeCommand.SetCommand>(TypeSet);
            await MapCommandAsync<SettingsCommand.TypeCommand.ShowCommand>(TypeShow);

            await MapCommandAsync<SettingsCommand.AuthCommand.SetCommand>(AuthSet);
            await MapCommandAsync<SettingsCommand.AuthCommand.ShowCommand>(AuthShow);

            await MapCommandAsync<SettingsCommand.EveryCommand.SetCommand>(EverySet);
            await MapCommandAsync<SettingsCommand.EveryCommand.ShowCommand>(EveryShow);

            await MapCommandAsync<SettingsCommand.ScheduleCommand.AddCommand>(ScheduleAdd);
            await MapCommandAsync<SettingsCommand.ScheduleCommand.EditCommand>(ScheduleEdit);
            await MapCommandAsync<SettingsCommand.ScheduleCommand.RemoveCommand>(ScheduleRemove);
            await MapCommandAsync<SettingsCommand.ScheduleCommand.ListCommand>(ScheduleList);

            Environment.Exit(0);
        }

        public async Task TypeSet(SettingsCommand.TypeCommand.SetCommand command)
        {
            var settings = await _settingsHelper.ReadSettings();

            settings.SettingsType = command.SettingsType;

            await _settingsHelper.WriteSettings(settings);

            Console.WriteLine("Тип настроек изменён.");
        }

        public async Task TypeShow(SettingsCommand.TypeCommand.ShowCommand command)
        {
            var settings = await _settingsHelper.ReadSettings();

            Console.WriteLine($"Тип настроек: {settings.SettingsType}");
        }

        public async Task AuthSet(SettingsCommand.AuthCommand.SetCommand command)
        {
            var settings = await _settingsHelper.ReadSettings();

            settings.AccessToken = command.AccessToken;

            await _settingsHelper.WriteSettings(settings);

            Console.WriteLine("Токен авторизации изменён.");
        }

        public async Task AuthShow(SettingsCommand.AuthCommand.ShowCommand command)
        {
            var settings = await _settingsHelper.ReadSettings();

            var token = string.IsNullOrWhiteSpace(settings.AccessToken)
                ? "отсутствует"
                : settings.AccessToken;

            Console.WriteLine($"Токен авторизации: {token}");
        }

        public void Reset(SettingsCommand.ResetCommand command)
        {
            _settingsHelper.ResetSettings();

            Console.WriteLine("Настройки сброшены.");
        }

        public async Task EverySet(SettingsCommand.EveryCommand.SetCommand command)
        {
            var settings = await _settingsHelper.ReadSettings();

            settings.Every!.StatusesTexts = command.StatusesTexts.ToList();
            settings.Every.Seconds = command.Seconds;

            await _settingsHelper.WriteSettings(settings);

            Console.WriteLine("Настройки Every изменены.");
        }

        public async Task EveryShow(SettingsCommand.EveryCommand.ShowCommand command)
        {
            var settings = await _settingsHelper.ReadSettings();

            Console.WriteLine($"Статусы меняются каждые {settings.Every.Seconds} секунд, список статусов:");

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

            Console.WriteLine("Расписание добавлено.");
        }

        public async Task ScheduleEdit(SettingsCommand.ScheduleCommand.EditCommand command)
        {
            var settings = await _settingsHelper.ReadSettings();

            var item = settings.Schedule.Items[command.Id - 1];

            item.StatusText = command.StatusText;
            item.Date = command.Date;
            item.Time = command.Time;

            await _settingsHelper.WriteSettings(settings);

            Console.WriteLine("Расписание изменено.");
        }

        public async Task ScheduleRemove(SettingsCommand.ScheduleCommand.RemoveCommand command)
        {
            var settings = await _settingsHelper.ReadSettings();

            if (command.Id.HasValue)
                settings.Schedule!.Items!.RemoveAt(command.Id.Value - 1);
            else
                settings.Schedule!.Items!.RemoveAll(_ => true);

            await _settingsHelper.WriteSettings(settings);

            Console.WriteLine("Расписание удалено.");
        }

        public async Task ScheduleList(SettingsCommand.ScheduleCommand.ListCommand command)
        {
            var settings = await _settingsHelper.ReadSettings();

            if (!settings.Schedule!.Items!.Any())
            {
                Console.Write("Расписания отсутствуют.");
                return;
            }

            for(int index = 0; index < settings.Schedule!.Items!.Count; index++)
            {
                var scheduleItem = settings.Schedule.Items[index];
                Console.WriteLine($"{index + 1}. Статус: {scheduleItem.StatusText}, Дата: {scheduleItem.Date:dd.MM.yyyy}, Время: {scheduleItem.Time}");
            }
        }
    }
}
