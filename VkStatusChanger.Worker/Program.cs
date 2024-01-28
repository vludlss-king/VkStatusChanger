using CommandLine;
using VkStatusChanger.Worker.Extensions;
using Microsoft.Extensions.Hosting;
using Serilog;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using VkStatusChanger.Worker.Contracts.Infrastructure;
using VkStatusChanger.Worker.Infrastructure;
using Microsoft.Extensions.Logging;
using VkStatusChanger.Worker.Commands;
using VkNet.Model;

[assembly: InternalsVisibleTo("VkStatusChanger.Worker.Tests")]

namespace VkStatusChanger;

internal class Program
{
    static async Task Main(string[] args)
    {
        if(!args.Any())
        {
            Console.WriteLine("Документация: https://github.com/vludlss-king/VkStatusChanger/blob/main/README.md");
            Console.ReadKey();
            return;
        }

        var host = CreateHost(args);
        await host.RunAsync();

        Console.ReadKey();
    }

    static IHost CreateHost(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddConfiguration(builder.Configuration);
        builder.Services.AddHttpClients();
        builder.Services.AddSerilog();

        var parserResult = Parser.Default.ParseVerbs<Routes.Start, Routes.Settings>(args);
        builder.Services.AddSingleton<ICustomParserResult, CustomParserResult>(provider => new CustomParserResult(parserResult));

        parserResult.WithParsed<Routes.Start>(command => builder.Services.AddJobScheduler());

        if (parserResult.TypeInfo.Current != typeof(Routes.Start))
        {
            builder.Services.AddCommands();
            builder.Services.AddCommandHandler();
        }

        var host = builder.Build();
        return host;
    }
}