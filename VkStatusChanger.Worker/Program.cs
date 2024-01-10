using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using VkStatusChanger.Worker.Extensions;
using VkStatusChanger.Worker.Models;
using VkStatusChanger.Worker.Models.Settings;
using Serilog;

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
                    builder.Services.AddConfiguration(builder.Configuration, settingsModel, inputArgs);
                    builder.Services.AddVkHttpClient();
                    builder.Services.AddJobScheduler(settingsModel);
                    builder.Services.AddSerilog(cfg =>
                    {
                        cfg.Enrich.FromLogContext()
                            .WriteTo.Console();
                    });

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
            string settingsJson = File.ReadAllText(settingsFileName);
            SettingsModel settingsModel = JsonConvert.DeserializeObject<SettingsModel>(settingsJson) ?? new SettingsModel();

            return settingsModel;
        }
    }
}