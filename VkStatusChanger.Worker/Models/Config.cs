using CommandLine;
using VkStatusChanger.Worker.Attributes;

namespace VkStatusChanger.Worker.Models
{
    [Verb("config")]
    [ChildVerbs(typeof(Every), typeof(Schedule))]
    internal class Config
    {
        [Verb("every")]
        [ChildVerbs(typeof(Show), typeof(Set))]
        internal class Every
        {
            [Verb("set")]
            internal class Set
            {
                [Option("statuses-texts", Required = true, Separator = ',')]
                public IEnumerable<string> StatusesTexts { get; set; }
                [Option("seconds", Required = true)]
                public int Seconds { get; set; }
            }

            [Verb("show")]
            internal class Show
            {

            }
        }

        [Verb("schedule")]
        [ChildVerbs(typeof(Add), typeof(Edit), typeof(Remove), typeof(List))]
        internal class Schedule
        {
            [Verb("add")]
            internal class Add
            {
                [Option("status-text", Required = true)]
                public string StatusText { get; set; }
                [Option("date", Required = true)]
                public DateTime Date { get; set; }
                [Option("time", Required = true)]
                public TimeSpan Time { get; set; }
            }

            [Verb("edit")]
            internal class Edit
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
            internal class Remove
            {
                [Option("id", Required = false)]
                public int? Id { get; set; }
            }

            [Verb("list")]
            internal class List
            {

            }
        }
    }
}
