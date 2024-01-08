using CommandLine;

namespace VkStatusChanger.Worker.Models
{
    public class InputArgs
    {
        [Option("application-id", Required = true)]
        public ulong ApplicationId { get; set; }
        [Option("login", Required = true)]
        public required string Login { get; set; }
        [Option("password", Required = true)]
        public required string Password { get; set; }
        [Option("two-factor-auth", Required = true)]
        public bool TwoFactorAuth { get; set; }
    }
}
