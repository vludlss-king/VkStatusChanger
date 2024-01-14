using CommandLine;

namespace VkStatusChanger.Worker.Contracts.Infrastructure
{
    public interface ICustomParserResult
    {
        ParserResult<object?> WithParsed<T>(Action<T> action);
        Task<ParserResult<object>> WithParsedAsync<T>(Func<T, Task> action);
    }
}
