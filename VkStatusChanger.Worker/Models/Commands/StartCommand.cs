using CommandLine;
using VkStatusChanger.Worker.Models.Commands.Common;

namespace VkStatusChanger.Worker.Models.Commands
{
    [Verb("start", HelpText = "Запустить смену статусов по указанным настройкам.")]
    internal class StartCommand : Command
    {
    }
}
