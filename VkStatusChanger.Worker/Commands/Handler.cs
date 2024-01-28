using Microsoft.Extensions.Hosting;
using VkStatusChanger.Worker.Contracts;
using VkStatusChanger.Worker.Contracts.Infrastructure;

namespace VkStatusChanger.Worker.Commands;

internal class Handler : IHostedService
{
    private readonly IEnumerable<ICommand> _commands;
    private readonly ICustomParserResult _parserResult;

    public Handler(IEnumerable<ICommand> commands, ICustomParserResult parserResult)
    {
        _commands = commands;
        _parserResult = parserResult;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (dynamic command in _commands)
        {
            await _parserResult.WithParsedAsync<dynamic>(async request =>
            {
                string output = await command.Execute(request);
                Console.WriteLine(output);
            });
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
