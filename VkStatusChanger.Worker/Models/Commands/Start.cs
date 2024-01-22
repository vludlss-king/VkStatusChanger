using CommandLine;
using VkStatusChanger.Worker.Models.Commands.Common;

namespace VkStatusChanger.Worker.Models.Commands
{
    internal partial class Command
    {
        [Verb("start", HelpText = "Запустить смену статусов по указанным настройкам.")]
        internal class Start : BaseCommand
        {
        }
    }
}
