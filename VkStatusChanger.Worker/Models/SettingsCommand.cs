using CommandLine;
using VkStatusChanger.Worker.Attributes;

namespace VkStatusChanger.Worker.Models
{
    [Verb("settings")]
    [ChildVerbs(typeof(EveryCommand), typeof(ScheduleCommand))]
    internal class SettingsCommand
    {
        [Verb("every")]
        [ChildVerbs(typeof(ShowCommand), typeof(SetCommand))]
        internal class EveryCommand
        {
            [Verb("set")]
            internal class SetCommand
            {
                [Option("statuses-texts", Required = true, Separator = ',')]
                public IEnumerable<string> StatusesTexts { get; set; }
                [Option("seconds", Required = true)]
                public int Seconds { get; set; }
            }

            [Verb("show")]
            internal class ShowCommand
            {

            }
        }

        [Verb("schedule")]
        [ChildVerbs(typeof(AddCommand), typeof(EditCommand), typeof(RemoveCommand), typeof(ListCommand))]
        internal class ScheduleCommand
        {
            [Verb("add")]
            internal class AddCommand
            {
                [Option("status-text", Required = true)]
                public string StatusText { get; set; }
                [Option("date", Required = true)]
                public DateTime Date { get; set; }
                [Option("time", Required = true)]
                public TimeSpan Time { get; set; }
            }

            [Verb("edit")]
            internal class EditCommand
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
            internal class RemoveCommand
            {
                [Option("id", Required = false)]
                public int? Id { get; set; }
            }

            [Verb("list")]
            internal class ListCommand
            {

            }
        }
    }
}
