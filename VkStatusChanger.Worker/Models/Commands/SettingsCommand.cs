using CommandLine;
using VkStatusChanger.Worker.Attributes;
using VkStatusChanger.Worker.Enums;
using VkStatusChanger.Worker.Models.Commands.Common;

namespace VkStatusChanger.Worker.Models.Commands
{
    [Verb("settings")]
    [ChildVerbs(typeof(ResetCommand), typeof(TypeCommand), typeof(AuthCommand), typeof(EveryCommand), typeof(ScheduleCommand))]
    internal class SettingsCommand : Command
    {
        [Verb("reset")]
        internal class ResetCommand : Command
        {

        }

        [Verb("type")]
        [ChildVerbs(typeof(SetCommand), typeof(ShowCommand))]
        internal class TypeCommand : Command
        {
            [Verb("set")]
            internal class SetCommand : Command
            {
                [Option("settings-type", Required = true, Default = SettingsType.Every)]
                public SettingsType SettingsType { get; set; }
            }

            [Verb("show")]
            internal class ShowCommand : Command
            {

            }
        }

        [Verb("auth")]
        [ChildVerbs(typeof(SetCommand), typeof(ShowCommand))]
        internal class AuthCommand : Command
        {
            [Verb("set")]
            internal class SetCommand : Command
            {
                [Option("access-token", Required = true)]
                public string? AccessToken { get; set; }
            }

            [Verb("show")]
            internal class ShowCommand : Command
            {

            }
        }

        [Verb("every")]
        [ChildVerbs(typeof(ShowCommand), typeof(SetCommand))]
        internal class EveryCommand : Command
        {
            [Verb("set")]
            internal class SetCommand : Command
            {
                [Option("statuses-texts", Required = true, Separator = ',')]
                public IEnumerable<string> StatusesTexts { get; set; }
                [Option("seconds", Required = true)]
                public int Seconds { get; set; }
            }

            [Verb("show")]
            internal class ShowCommand : Command
            {

            }
        }

        [Verb("schedule")]
        [ChildVerbs(typeof(AddCommand), typeof(EditCommand), typeof(RemoveCommand), typeof(ListCommand))]
        internal class ScheduleCommand : Command
        {
            [Verb("add")]
            internal class AddCommand : Command
            {
                [Option("status-text", Required = true)]
                public string StatusText { get; set; }
                [Option("date", Required = true)]
                public DateTime Date { get; set; }
                [Option("time", Required = true)]
                public TimeSpan Time { get; set; }
            }

            [Verb("edit")]
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

            [Verb("remove")]
            internal class RemoveCommand : Command
            {
                [Option("id", Required = false)]
                public int? Id { get; set; }
            }

            [Verb("list")]
            internal class ListCommand : Command
            {

            }
        }
    }
}
