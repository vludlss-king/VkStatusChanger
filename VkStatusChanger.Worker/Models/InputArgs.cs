using CommandLine;

namespace VkStatusChanger.Worker.Models
{
    public class InputArgs
    {
        [Option("access-token", Required = false)]
        public required string AccessToken { get; set; }
    }
}
