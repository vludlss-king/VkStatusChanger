using CommandLine;

namespace VkStatusChanger.Worker.Models
{
    public class Authorization
    {
        /// <summary>
        /// Токен для входа в клиент VK API
        /// </summary>
        [Option("access-token", Required = false)]
        public required string? AccessToken { get; set; }
    }
}
