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

        public void MapCommand<TCommand>(Func<TCommand, string> func)
            where TCommand : Command
            => _parserResult.WithParsed<TCommand>(command =>
            {
                var output = func(command);
                Console.WriteLine(output);
            });

        public async Task MapCommandAsync<TCommand>(Func<TCommand, Task<string>> func)
            where TCommand : Command
        {
            Func<TCommand, Task> funcWrapper = async command =>
            {
                var output = await func(command);
                Console.WriteLine(output);
            };

            await _parserResult.WithParsedAsync(funcWrapper);
        }

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
