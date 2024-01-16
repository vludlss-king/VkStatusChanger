using CommandLine;
using VkStatusChanger.Worker.Extensions;
using Microsoft.Extensions.Hosting;
using Serilog;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using VkStatusChanger.Worker.Models.Commands;
using VkStatusChanger.Worker.Contracts.Infrastructure;
using VkStatusChanger.Worker.Infrastructure;

[assembly: InternalsVisibleTo("VkStatusChanger.Worker.Tests")]

namespace VkStatusChanger
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddConfiguration(builder.Configuration);
            builder.Services.AddHttpClients();
            builder.Services.AddSerilog(cfg =>
            {
                cfg.Enrich.FromLogContext()
                    .WriteTo.Console();
            });

            var parserResult = Parser.Default.ParseVerbs<StartCommand, SettingsCommand>(args);
            builder.Services.AddSingleton<ICustomParserResult, CustomParserResult>(provider => new CustomParserResult(parserResult));

            parserResult.WithParsed<StartCommand>(command => builder.Services.AddJobScheduler());

            if(parserResult.TypeInfo.Current != typeof(StartCommand))
                builder.Services.AddControllers();

            var host = builder.Build();
            await host.RunAsync();

            Console.ReadKey();
        }
    }
}