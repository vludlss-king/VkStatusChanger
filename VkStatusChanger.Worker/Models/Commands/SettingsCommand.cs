using CommandLine;
using VkStatusChanger.Worker.Attributes;
using VkStatusChanger.Worker.Enums;
using VkStatusChanger.Worker.Models.Commands.Common;

namespace VkStatusChanger.Worker.Models.Commands
{
    [Verb("settings", HelpText = "Настройки.")]
    [ChildVerbs(typeof(ResetCommand), typeof(TypeCommand), typeof(AuthCommand), typeof(EveryCommand), typeof(ScheduleCommand))]
    internal class SettingsCommand : Command
    {
        [Verb("reset", HelpText = "Сбросить настройки.")]
        internal class ResetCommand : Command
        {

        }

        [Verb("type", HelpText = "Тип настроек.")]
        [ChildVerbs(typeof(SetCommand), typeof(ShowCommand))]
        internal class TypeCommand : Command
        {
            [Verb("set", HelpText = "Установить тип настроек.")]
            internal class SetCommand : Command
            {
                [Option("settings-type", Required = true, Default = SettingsType.Every)]
                public SettingsType SettingsType { get; set; }
            }

            [Verb("show", HelpText = "Показать текущий тип настроек.")]
            internal class ShowCommand : Command
            {

            }
        }

        [Verb("auth", HelpText = "Авторизация.")]
        [ChildVerbs(typeof(SetCommand), typeof(ShowCommand))]
        internal class AuthCommand : Command
        {
            [Verb("set", HelpText = "Авторизоваться.")]
            internal class SetCommand : Command
            {
                [Option("access-token", Required = true, HelpText = "Токен доступа Standalone-прлиожения.")]
                public string? AccessToken { get; set; }
            }

            [Verb("show", HelpText = "Показать текущую авторизацию.")]
            internal class ShowCommand : Command
            {

            }
        }

        [Verb("every", HelpText = "Тип настроек Every (менять статусы по очереди каждые N секунд).")]
        [ChildVerbs(typeof(ShowCommand), typeof(SetCommand))]
        internal class EveryCommand : Command
        {
            [Verb("set", HelpText = "Установить статусы.")]
            internal class SetCommand : Command
            {
                [Option("statuses-texts", Required = true, Separator = ',')]
                public IEnumerable<string> StatusesTexts { get; set; }
                [Option("seconds", Required = true)]
                public int Seconds { get; set; }
            }

            [Verb("show", HelpText = "Показать текущие статусы.")]
            internal class ShowCommand : Command
            {

            }
        }

        [Verb("schedule", HelpText = "Тип настроек Schedule (менять статусы по датам и времени).")]
        [ChildVerbs(typeof(AddCommand), typeof(EditCommand), typeof(RemoveCommand), typeof(ListCommand))]
        internal class ScheduleCommand : Command
        {
            [Verb("add", HelpText = "Добавить статус.")]
            internal class AddCommand : Command
            {
                [Option("status-text", Required = true)]
                public string StatusText { get; set; }
                [Option("date", Required = true)]
                public DateTime Date { get; set; }
                [Option("time", Required = true)]
                public TimeSpan Time { get; set; }
            }

            [Verb("edit", HelpText = "Редактировать статус.")]
            internal class EditCommand : Command
            {
                [Option("id", Required = true)]
                public int Id { get; set; }
                [Option("status-text", Required = false)]
                public string StatusText { get; set; }
                [Option("date", Required = false)]
                public DateTime Date { get; set; }
                [Option("time", Required = false)]
                public TimeSpan Time { get; set; }
            }

            [Verb("remove", HelpText = "Удалить статус.")]
            internal class RemoveCommand : Command
            {
                [Option("id", Required = false)]
                public int? Id { get; set; }
            }

            [Verb("list", HelpText = "Показать список текущих статусов.")]
            internal class ListCommand : Command
            {

            }
        }
    }
}
