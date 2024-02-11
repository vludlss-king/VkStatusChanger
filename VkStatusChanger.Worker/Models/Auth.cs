using CommandLine;

namespace VkStatusChanger.Worker.Models;

public class Auth
{
    /// <summary>
    /// Токен для входа в клиент VK API
    /// </summary>
    [Option("access-token", Required = false)]
    public string? AccessToken { get; set; }
}
