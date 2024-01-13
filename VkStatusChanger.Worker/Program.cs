using CommandLine;
using VkStatusChanger.Worker.Extensions;
using Microsoft.Extensions.Hosting;
using Serilog;
using Microsoft.Extensions.DependencyInjection;
using VkStatusChanger.Worker.HostedServices;
using System.Runtime.CompilerServices;
using VkStatusChanger.Worker.Models.Commands;

[assembly: InternalsVisibleTo("VkStatusChanger.Worker.Tests")]

namespace VkStatusChanger
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddConfiguration(builder.Configuration);
            builder.Services.AddVkHttpClient();
            builder.Services.AddControllers();
            builder.Services.AddSerilog(cfg =>
            {
                cfg.Enrich.FromLogContext()
                    .WriteTo.Console();
            });

            var parserResult = Parser.Default.ParseVerbs<StartCommand, SettingsCommand>(args);
            builder.Services.AddSingleton(provider => parserResult);

            parserResult.WithParsed<StartCommand>(command => builder.Services.AddJobScheduler());

            if(parserResult.TypeInfo.Current != typeof(StartCommand))
                builder.Services.AddHostedService<CommandHostedService>();

            var host = builder.Build();
            await host.RunAsync();

            Console.ReadKey();
        }
    }
}