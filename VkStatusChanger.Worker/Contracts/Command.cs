using FluentValidation.Results;
using System.Diagnostics.CodeAnalysis;

namespace VkStatusChanger.Worker.Contracts
{
    internal abstract class Command<T> : ICommand<T>
    {
        [AllowNull]
        protected ValidationResult ModelState { get; set; }

        public abstract Task<string> Execute(T request);
    }
}
