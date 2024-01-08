using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using VkStatusChanger.Worker.Extensions;
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
                    Initialize();

                    var builder = Host.CreateApplicationBuilder(args);

                    var settingsModel = ReadSettings();
                    builder.Services.AddSingleton(provider => Options.Create(settingsModel!));

                    builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddUserSecrets<InputArgs>();
                    builder.Services.AddSingleton(provider =>
                    {
                        var env = provider.GetRequiredService<IHostEnvironment>();
                        var configuration = provider.GetRequiredService<IConfiguration>();

                        if (env.IsDevelopment())
                            return Options.Create(configuration.Get<InputArgs>()!);
                        else
                            return Options.Create(inputArgs);
                    });

                    builder.Services.AddVkHttpClient();

                    builder.Services.AddJobScheduler(settingsModel!);

                    var host = builder.Build();
                    await host.RunAsync();
                });
        }

        static void Initialize()
        {
            if(!File.Exists(settingsFileName))
                File.Create(settingsFileName).Dispose();
        }

        static SettingsModel ReadSettings()
        {
            var settingsJson = File.ReadAllText(settingsFileName);
            var settingsModel = JsonConvert.DeserializeObject<SettingsModel>(settingsJson) ?? new SettingsModel();

            return settingsModel;
        }
    }
}