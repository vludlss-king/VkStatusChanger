using CommandLine;

namespace VkStatusChanger.Worker.Models
{
    public class InputArgs
    {
        [Option("application-id", Required = false)]
        public ulong ApplicationId { get; set; }
        [Option("login", Required = false)]
        public required string Login { get; set; }
        [Option("password", Required = false)]
        public required string Password { get; set; }
        [Option("two-factor-auth", Required = false)]
        public bool TwoFactorAuth { get; set; }
    }
}
