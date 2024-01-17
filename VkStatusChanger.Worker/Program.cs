using CommandLine;
using VkStatusChanger.Worker.Extensions;
using Microsoft.Extensions.Hosting;
using Serilog;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using VkStatusChanger.Worker.Models.Commands;
using VkStatusChanger.Worker.Contracts.Infrastructure;
using VkStatusChanger.Worker.Infrastructure;
using CommandLine.Text;
using Microsoft.Extensions.Logging;
using Serilog.Events;

[assembly: InternalsVisibleTo("VkStatusChanger.Worker.Tests")]

namespace VkStatusChanger
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            if(!args.Any())
            {
                Console.WriteLine("С приложением можно работать только через командную строчку, пожалуйста, прочитай документацию.");
                Console.WriteLine("https://github.com/vludlss-king/VkStatusChanger/blob/main/README.md");
                Console.ReadKey();
                return;
            }

            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddConfiguration(builder.Configuration);
            builder.Services.AddHttpClients();
            builder.Services.AddSerilog();

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