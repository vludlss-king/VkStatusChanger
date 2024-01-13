using CommandLine;
using VkStatusChanger.Worker.Extensions;
using VkStatusChanger.Worker.Models;
using Microsoft.Extensions.Hosting;
using Serilog;
using Microsoft.Extensions.DependencyInjection;
using VkStatusChanger.Worker.HostedServices;

namespace VkStatusChanger
{
    internal class Program
    {
        const string settingsFileName = "settings.json";

        static async Task Main(string[] args)
        {
            var parserResult = Parser.Default.ParseVerbs<Start, Config>(args);

            Initialize();
            var builder = Host.CreateApplicationBuilder();

            builder.Services.AddConfiguration(builder.Configuration);
            builder.Services.AddVkHttpClient();
            builder.Services.AddControllers();
            builder.Services.AddSerilog(cfg =>
            {
                cfg.Enrich.FromLogContext()
                    .WriteTo.Console();
            });

            builder.Services.AddSingleton(provider => parserResult);

            parserResult.WithParsed<Start>(command => builder.Services.AddJobScheduler());

            if(parserResult.TypeInfo.Current != typeof(Start))
                builder.Services.AddHostedService<CommandHostedService>();

            var host = builder.Build();
            await host.RunAsync();

            Console.ReadKey();
        }

        private static void Initialize()
        {
            if (!File.Exists(settingsFileName))
                File.Create(settingsFileName).Dispose();
        }
    }
}