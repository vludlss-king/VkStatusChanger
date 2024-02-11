using FluentValidation.Results;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace VkStatusChanger.Worker.Contracts
{
    internal abstract class Command<T> : ICommand<T>
    {
        [AllowNull]
        private ValidationResult _modelState;

        [AllowNull]
        protected ValidationResult ModelState
        {
            get => _modelState;
            set => _modelState = value ?? new ValidationResult { };
        }

        public abstract Task<string> Execute(T request);

        public Task<string> BadCommand()
            => Task.FromResult(string.Join(",\n", ModelState.Errors.Select(error => error.ErrorMessage)));

        public Task<string> Ok(object output)
            => Task.FromResult(output.ToString() ?? string.Empty);
    }
}
