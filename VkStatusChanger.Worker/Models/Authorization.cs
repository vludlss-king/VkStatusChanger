using CommandLine;

namespace VkStatusChanger.Worker.Models
{
    public class Authorization
    {
        [Option("access-token", Required = false)]
        public required string? AccessToken { get; set; }
    }
}
