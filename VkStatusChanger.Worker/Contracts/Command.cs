using FluentValidation.Results;
using System.Diagnostics.CodeAnalysis;

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

        public string BadCommand()
            => string.Join(",\n", ModelState.Errors.Select(error => error.ErrorMessage));
    }
}
