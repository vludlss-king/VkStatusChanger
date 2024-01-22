using System.Text;
using VkStatusChanger.Worker.Contracts.Infrastructure;
using VkStatusChanger.Worker.Controllers.Common;
using VkStatusChanger.Worker.Models.Commands;
using VkStatusChanger.Worker.Models.UserSettings;

namespace VkStatusChanger.Worker.Controllers
{
    internal class CommandController : BaseController
    {
        private readonly ISettingsManager _settingsManager;

        public CommandController(ICustomParserResult parserResult, ISettingsManager settingsManager)
            : base(parserResult)
        {
            _settingsManager = settingsManager;
        }

        public override async Task ExecuteCommand()
        {
            MapCommand<SettingsCommand.ResetCommand>(Execute);
            await MapCommandAsync<SettingsCommand.TypeCommand.SetCommand>(Execute);
            await MapCommandAsync<SettingsCommand.TypeCommand.ShowCommand>(Execute);

            await MapCommandAsync<SettingsCommand.AuthCommand.SetCommand>(Execute);
            await MapCommandAsync<SettingsCommand.AuthCommand.ShowCommand>(Execute);

            await MapCommandAsync<SettingsCommand.EveryCommand.SetCommand>(Execute);
            await MapCommandAsync<SettingsCommand.EveryCommand.ShowCommand>(Execute);

            await MapCommandAsync<SettingsCommand.ScheduleCommand.AddCommand>(Execute);
            await MapCommandAsync<SettingsCommand.ScheduleCommand.EditCommand>(Execute);
            await MapCommandAsync<SettingsCommand.ScheduleCommand.RemoveCommand>(Execute);
            await MapCommandAsync<SettingsCommand.ScheduleCommand.ListCommand>(Execute);

            Environment.Exit(0);
        }

        /// <summary>
        /// Выполнить команду: settings reset
        /// </summary>
        public string Execute(SettingsCommand.ResetCommand command)
        {
            _settingsManager.Reset();

            var output = "Настройки сброшены.";
            return output;
        }

        /// <summary>
        /// Выполнить команду: settings type set
        /// </summary>
        public async Task<string> Execute(SettingsCommand.TypeCommand.SetCommand command)
        {
            var settings = await _settingsManager.Read();

            settings.Type = command.SettingsType;

            await _settingsManager.Write(settings);

            var output = "Тип настроек изменён.";
            return output;
        }

        /// <summary>
        /// Выполнить команду: settings type show
        /// </summary>
        public async Task<string> Execute(SettingsCommand.TypeCommand.ShowCommand command)
        {
            var settings = await _settingsManager.Read();

            var output = $"Тип настроек: {settings.Type}";
            return output;
        }

        /// <summary>
        /// Выполнить команду: settings auth set
        /// </summary>
        public async Task<string> Execute(SettingsCommand.AuthCommand.SetCommand command)
        {
            var settings = await _settingsManager.Read();

            settings.AccessToken = command.AccessToken;

            await _settingsManager.Write(settings);

            var output = "Токен авторизации изменён.";
            return output;
        }

        /// <summary>
        /// Выполнить команду: settings auth show
        /// </summary>
        public async Task<string> Execute(SettingsCommand.AuthCommand.ShowCommand command)
        {
            var settings = await _settingsManager.Read();

            var token = string.IsNullOrWhiteSpace(settings.AccessToken)
                ? "отсутствует"
                : settings.AccessToken;

            var output = $"Токен авторизации: {token}";
            return output;
        }

        /// <summary>
        /// Выполнить команду: settings every set
        /// </summary>
        public async Task<string> Execute(SettingsCommand.EveryCommand.SetCommand command)
        {
            var settings = await _settingsManager.Read();

            settings.Every.StatusesTexts = command.StatusesTexts.ToList();
            settings.Every.Seconds = command.Seconds;

            await _settingsManager.Write(settings);

            var output = "Настройки Every изменены.";
            return output;
        }

        /// <summary>
        /// Выполнить команду: settings every show
        /// </summary>
        public async Task<string> Execute(SettingsCommand.EveryCommand.ShowCommand command)
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

        /// <summary>
        /// Выполнить команду: settings schedule add
        /// </summary>
        public async Task<string> Execute(SettingsCommand.ScheduleCommand.AddCommand command)
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

        /// <summary>
        /// Выполнить команду: settings schedule edit
        /// </summary>
        public async Task<string> Execute(SettingsCommand.ScheduleCommand.EditCommand command)
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

        /// <summary>
        /// Выполнить команду: settings schedule remove
        /// </summary>
        public async Task<string> Execute(SettingsCommand.ScheduleCommand.RemoveCommand command)
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

        /// <summary>
        /// Выполнить команду: settings schedule list
        /// </summary>
        public async Task<string> Execute(SettingsCommand.ScheduleCommand.ListCommand command)
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
