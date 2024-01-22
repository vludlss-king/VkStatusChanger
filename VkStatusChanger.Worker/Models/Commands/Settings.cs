using CommandLine;
using VkStatusChanger.Worker.Attributes;
using VkStatusChanger.Worker.Enums;
using VkStatusChanger.Worker.Models.Commands.Common;

namespace VkStatusChanger.Worker.Models.Commands
{
    internal partial class Command
    {
        [Verb("settings", HelpText = "Настройки.")]
        [ChildVerbs(typeof(Reset), typeof(Type), typeof(Auth), typeof(Every), typeof(Schedule))]
        internal class Settings : BaseCommand
        {
            [Verb("reset", HelpText = "Сбросить настройки.")]
            internal class Reset : BaseCommand
            {

            }

            [Verb("type", HelpText = "Тип настроек.")]
            [ChildVerbs(typeof(Set), typeof(Show))]
            internal class Type : BaseCommand
            {
                [Verb("set", HelpText = "Установить тип настроек.")]
                internal class Set : BaseCommand
                {
                    [Option("settings-type", Required = true, Default = SettingsType.Every)]
                    public SettingsType SettingsType { get; set; }
                }

                [Verb("show", HelpText = "Показать текущий тип настроек.")]
                internal class Show : BaseCommand
                {

                }
            }

            [Verb("auth", HelpText = "Авторизация.")]
            [ChildVerbs(typeof(Set), typeof(Show))]
            internal class Auth : BaseCommand
            {
                [Verb("set", HelpText = "Авторизоваться.")]
                internal class Set : BaseCommand
                {
                    [Option("access-token", Required = true, HelpText = "Токен доступа Standalone-прлиожения.")]
                    public string? AccessToken { get; set; }
                }

                [Verb("show", HelpText = "Показать текущую авторизацию.")]
                internal class Show : BaseCommand
                {

                }
            }

            [Verb("every", HelpText = "Тип настроек Every (менять статусы по очереди каждые N секунд).")]
            [ChildVerbs(typeof(Show), typeof(Set))]
            internal class Every : BaseCommand
            {
                [Verb("set", HelpText = "Установить статусы.")]
                internal class Set : BaseCommand
                {
                    [Option("statuses-texts", Required = true, Separator = ',')]
                    public IEnumerable<string> StatusesTexts { get; set; }
                    [Option("seconds", Required = true)]
                    public int Seconds { get; set; }
                }

                [Verb("show", HelpText = "Показать текущие статусы.")]
                internal class Show : BaseCommand
                {

                }
            }

            [Verb("schedule", HelpText = "Тип настроек Schedule (менять статусы по датам и времени).")]
            [ChildVerbs(typeof(Add), typeof(Edit), typeof(Remove), typeof(List))]
            internal class Schedule : BaseCommand
            {
                [Verb("add", HelpText = "Добавить статус.")]
                internal class Add : BaseCommand
                {
                    [Option("status-text", Required = true)]
                    public string StatusText { get; set; }
                    [Option("date", Required = true)]
                    public DateTime Date { get; set; }
                    [Option("time", Required = true)]
                    public TimeSpan Time { get; set; }
                }

                [Verb("edit", HelpText = "Редактировать статус.")]
                internal class Edit : BaseCommand
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
                internal class Remove : BaseCommand
                {
                    [Option("id", Required = false)]
                    public int? Id { get; set; }
                }

                [Verb("list", HelpText = "Показать список текущих статусов.")]
                internal class List : BaseCommand
                {

                }
            }
        }
    }
}
