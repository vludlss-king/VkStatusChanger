using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using VkNet;
using VkNet.Abstractions;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkStatusChanger.Worker.Models;
using VkStatusChanger.Worker.Models.Settings;

namespace VkStatusChanger
{
    internal class Program
    {
        const string settingsFileName = "settings.json";

        static async Task Main(string[] args)
        {
            await Parser.Default.ParseArguments<InputArgs>(args)
                .WithParsedAsync(async inputArgs =>
                {
                    Initialize(inputArgs);

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

                    builder.Services.AddSingleton(provider =>
                    {
                        var settingsJson = File.ReadAllText(settingsFileName);
                        var settingsModel = JsonConvert.DeserializeObject<SettingsModel>(settingsJson);

                        return Options.Create(settingsModel!);
                    });

                    var host = builder.Build();
                    await host.RunAsync();
                });
        }

        static void Initialize(InputArgs inputArgs)
        {
            if(!File.Exists(settingsFileName))
                File.Create(settingsFileName).Dispose();
        }
    }
}