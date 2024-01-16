using CommandLine;

namespace VkStatusChanger.Worker.Models
{
    public class Authorization
    {
        /// <summary>
        /// Токен для входа в клиент VK API
        /// </summary>
        [Option("access-token", Required = false)]
        public string? AccessToken { get; set; }
    }
}
