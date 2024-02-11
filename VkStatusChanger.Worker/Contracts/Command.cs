using FluentValidation.Results;

namespace VkStatusChanger.Worker.Contracts
{
    internal abstract class Command<T> : ICommand<T>
    {
        protected ValidationResult ModelState { get; set; } = new();

        public abstract Task<string> Execute(T request);

        public Task<string> BadCommand()
            => Task.FromResult(string.Join(",\n", ModelState.Errors.Select(error => error.ErrorMessage)));

        public Task<string> Ok(object output)
            => Task.FromResult(output.ToString() ?? string.Empty);
    }
}
