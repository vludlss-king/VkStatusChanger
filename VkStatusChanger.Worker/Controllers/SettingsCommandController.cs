using System.Text;
using VkStatusChanger.Worker.Contracts.Infrastructure;
using VkStatusChanger.Worker.Controllers.Common;
using VkStatusChanger.Worker.Models.Commands;
using VkStatusChanger.Worker.Models.UserSettings;

namespace VkStatusChanger.Worker.Controllers
{
    internal class SettingsCommandController : BaseController
    {
        private readonly ISettingsManager _settingsManager;

        public SettingsCommandController(ICustomParserResult parserResult, ISettingsManager settingsManager)
            : base(parserResult)
        {
            _settingsManager = settingsManager;
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

        public async Task<string> TypeSet(SettingsCommand.TypeCommand.SetCommand command)
        {
            var settings = await _settingsManager.Read();

            settings.Type = command.SettingsType;

            await _settingsManager.Write(settings);

            var output = "Тип настроек изменён.";
            return output;
        }

        public async Task<string> TypeShow(SettingsCommand.TypeCommand.ShowCommand command)
        {
            var settings = await _settingsManager.Read();

            var output = $"Тип настроек: {settings.Type}";
            return output;
        }

        public async Task<string> AuthSet(SettingsCommand.AuthCommand.SetCommand command)
        {
            var settings = await _settingsManager.Read();

            settings.AccessToken = command.AccessToken;

            await _settingsManager.Write(settings);

            var output = "Токен авторизации изменён.";
            return output;
        }

        public async Task<string> AuthShow(SettingsCommand.AuthCommand.ShowCommand command)
        {
            var settings = await _settingsManager.Read();

            var token = string.IsNullOrWhiteSpace(settings.AccessToken)
                ? "отсутствует"
                : settings.AccessToken;

            var output = $"Токен авторизации: {token}";
            return output;
        }

        public string Reset(SettingsCommand.ResetCommand command)
        {
            _settingsManager.Reset();

            var output = "Настройки сброшены.";
            return output;
        }

        public async Task<string> EverySet(SettingsCommand.EveryCommand.SetCommand command)
        {
            var settings = await _settingsManager.Read();

            settings.Every.StatusesTexts = command.StatusesTexts.ToList();
            settings.Every.Seconds = command.Seconds;

            await _settingsManager.Write(settings);

            var output = "Настройки Every изменены.";
            return output;
        }

        public async Task<string> EveryShow(SettingsCommand.EveryCommand.ShowCommand command)
        {
            var settings = await _settingsManager.Read();

            var outputBuilder = new StringBuilder();
            outputBuilder.AppendLine($"Статусы меняются каждые {settings.Every.Seconds} секунд, список статусов:");

            if (!settings.Every.StatusesTexts.Any())
            {
                outputBuilder.AppendLine("отсутствуют");
            }
            else
            {
                for(int index = 0; index < settings.Every.StatusesTexts.Count; index++)
                {
                    var statusText = settings.Every.StatusesTexts[index];
                    outputBuilder.AppendLine($"{index + 1}. {statusText}");
                }
            }

            return outputBuilder.ToString();
        }

        public async Task<string> ScheduleAdd(SettingsCommand.ScheduleCommand.AddCommand command)
        {
            var settings = await _settingsManager.Read();

            var scheduleItem = new ScheduleItem
            {
                StatusText = command.StatusText,
                Date = command.Date,
                Time = command.Time,
            };
            settings.Schedule.Items.Add(scheduleItem);

            await _settingsManager.Write(settings);

            var output = "Расписание добавлено.";
            return output;
        }

        public async Task<string> ScheduleEdit(SettingsCommand.ScheduleCommand.EditCommand command)
        {
            var settings = await _settingsManager.Read();

            var item = settings.Schedule.Items[command.Id - 1];

            item.StatusText = command.StatusText;
            item.Date = command.Date;
            item.Time = command.Time;

            await _settingsManager.Write(settings);

            var output = "Расписание изменено.";
            return output;
        }

        public async Task<string> ScheduleRemove(SettingsCommand.ScheduleCommand.RemoveCommand command)
        {
            var settings = await _settingsManager.Read();

            if (command.Id.HasValue)
                settings.Schedule.Items.RemoveAt(command.Id.Value - 1);
            else
                settings.Schedule.Items.RemoveAll(_ => true);

            await _settingsManager.Write(settings);

            var output = "Расписание удалено.";
            return output;
        }

        public async Task<string> ScheduleList(SettingsCommand.ScheduleCommand.ListCommand command)
        {
            var settings = await _settingsManager.Read();

            string output = "";
            if (!settings.Schedule.Items.Any())
                output = "Расписания отсутствуют.";
            else
                for(int index = 0; index < settings.Schedule.Items.Count; index++)
                {
                    var scheduleItem = settings.Schedule.Items[index];
                    output = $"{index + 1}. Статус: {scheduleItem.StatusText}, Дата: {scheduleItem.Date:dd.MM.yyyy}, Время: {scheduleItem.Time}";
                }

            return output;
        }
    }
}
