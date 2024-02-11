using CommandLine;
using VkStatusChanger.Worker.Attributes;
using VkStatusChanger.Worker.Enums;

namespace VkStatusChanger.Worker.Commands;

internal class Routes
{
    [Verb("start", HelpText = "Запустить смену статусов по указанным настройкам.")]
    internal class Start : Route
    {
    }

    [Verb("settings", HelpText = "Настройки.")]
    [ChildVerbs(typeof(Reset), typeof(Type), typeof(Auth), typeof(Every), typeof(Schedule))]
    internal class Settings : Route
    {
        [Verb("reset", HelpText = "Сбросить настройки.")]
        internal class Reset : Route
        {
        }

        [Verb("type", HelpText = "Тип настроек.")]
        [ChildVerbs(typeof(Set), typeof(Show))]
        internal class Type : Route
        {
            [Verb("set", HelpText = "Установить тип настроек.")]
            internal class Set : Route
            {
                [Option("settings-type", Required = true, Default = SettingsType.Every)]
                public SettingsType SettingsType { get; set; }
            }

            [Verb("show", HelpText = "Показать текущий тип настроек.")]
            internal class Show : Route
            {
            }
        }

        [Verb("auth", HelpText = "Авторизация.")]
        [ChildVerbs(typeof(Set), typeof(Show))]
        internal class Auth : Route
        {
            [Verb("set", HelpText = "Авторизоваться.")]
            internal class Set : Route
            {
                [Option("access-token", Required = true, HelpText = "Токен доступа Standalone-прлиожения.")]
                public string? AccessToken { get; set; }
            }

            [Verb("show", HelpText = "Показать текущую авторизацию.")]
            internal class Show : Route
            {
            }
        }

        [Verb("every", HelpText = "Тип настроек Every (менять статусы по очереди каждые N секунд).")]
        [ChildVerbs(typeof(Show), typeof(Set))]
        internal class Every : Route
        {
            [Verb("set", HelpText = "Установить статусы.")]
            internal class Set : Route
            {
                [Option("statuses-texts", Required = true, Separator = ',')]
                public IEnumerable<string> Statuses { get; set; }
                [Option("seconds", Required = true)]
                public int Seconds { get; set; }
            }

            [Verb("show", HelpText = "Показать текущие статусы.")]
            internal class Show : Route
            {
            }
        }

        [Verb("schedule", HelpText = "Тип настроек Schedule (менять статусы по датам и времени).")]
        [ChildVerbs(typeof(Add), typeof(Edit), typeof(Remove), typeof(List))]
        internal class Schedule : Route
        {
            [Verb("add", HelpText = "Добавить статус.")]
            internal class Add : Route
            {
                [Option("status-text", Required = true)]
                public string Status { get; set; }
                [Option("date", Required = true)]
                public DateTime Date { get; set; }
                [Option("time", Required = true)]
                public TimeSpan Time { get; set; }
            }

            [Verb("edit", HelpText = "Редактировать статус.")]
            internal class Edit : Route
            {
                [Option("id", Required = true)]
                public int Id { get; set; }
                [Option("status-text", Required = false)]
                public string Status { get; set; }
                [Option("date", Required = false)]
                public DateTime Date { get; set; }
                [Option("time", Required = false)]
                public TimeSpan Time { get; set; }
            }

            [Verb("remove", HelpText = "Удалить статус.")]
            internal class Remove : Route
            {
                [Option("id", Required = false)]
                public int? Id { get; set; }
            }

            [Verb("list", HelpText = "Показать список текущих статусов.")]
            internal class List : Route
            {
            }
        }
    }
}
