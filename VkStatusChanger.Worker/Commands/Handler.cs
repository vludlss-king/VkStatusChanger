using FluentValidation;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using VkStatusChanger.Worker.Contracts;
using VkStatusChanger.Worker.Contracts.Infrastructure;
using VkStatusChanger.Worker.Helpers;

namespace VkStatusChanger.Worker.Commands;

internal class Handler : IHostedService
{
    private readonly IEnumerable<ICommand> _commands;
    private readonly IEnumerable<IValidator> _validators;
    private readonly ICustomParserResult _parserResult;

    public Handler(IEnumerable<ICommand> commands, IEnumerable<IValidator> validators, ICustomParserResult parserResult)
    {
        _commands = commands;
        _validators = validators;
        _parserResult = parserResult;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _parserResult.WithParsedAsync<dynamic>(async request =>
        {
            foreach (dynamic command in _commands)
            {
                if (command.GetType().GetInterface("ICommand`1").GenericTypeArguments[0].FullName == request.GetType().FullName)
                {
                    dynamic validator = _validators.FirstOrDefault(validator => validator.GetType().GetInterface("IValidator`1").GenericTypeArguments[0].FullName == request.GetType().FullName);
                    if(validator is not null)
                    {
                        var validationResult = await validator.ValidateAsync(request);
                        ReflectionHelper.SetPrivatePropertyValue(command, "ModelState", validationResult);
                    }

                    string output = await command.Execute(request);
                    Console.WriteLine(output);
                    break;
                }
            }
        });

        Environment.Exit(0);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
