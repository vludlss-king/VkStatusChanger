using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VkNet;
using VkNet.Abstractions;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkStatusChanger.Worker.Models;

namespace VkStatusChanger
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await Parser.Default.ParseArguments<InputArgs>(args)
                .WithParsedAsync(async inputArgs =>
                {
                    var builder = Host.CreateApplicationBuilder(args);

                    builder.Services.AddScoped<IVkApi>(provider =>
                    {
                        var vkApi = new VkApi();

                        var authParams = new ApiAuthParams
                        {
                            ApplicationId = inputArgs.ApplicationId,
                            Login = inputArgs.Login,
                            Password = inputArgs.Password,
                            Settings = Settings.Status,
                        };

                        if (inputArgs.TwoFactorAuth)
                        {
                            authParams.TwoFactorAuthorization = () =>
                            {
                                Console.WriteLine("Введите код двух факторной авторизации");
                                return Console.ReadLine()!.Trim();
                            };
                        }

                        vkApi.Authorize(authParams);

                        return vkApi;
                    });

                    var host = builder.Build();
                    await host.RunAsync();
                });
        }
    }
}