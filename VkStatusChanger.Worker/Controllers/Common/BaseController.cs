using CommandLine;
using Microsoft.Extensions.Hosting;
using VkStatusChanger.Worker.Contracts.Infrastructure;
using VkStatusChanger.Worker.Models.Commands.Common;

namespace VkStatusChanger.Worker.Controllers.Common
{
    internal abstract class BaseController : IHostedService
    {
        private readonly ICustomParserResult _parserResult;


        public BaseController(ICustomParserResult parserResult)
        {
            _parserResult = parserResult;    
        }

        public abstract Task ExecuteCommand();

        public void MapCommand<TCommand>(Action<TCommand> action)
            where TCommand : Command
            => _parserResult.WithParsed(action);

        public async Task MapCommandAsync<TCommand>(Func<TCommand, Task> action)
            where TCommand : Command
            => await _parserResult.WithParsedAsync(action);

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await ExecuteCommand();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
