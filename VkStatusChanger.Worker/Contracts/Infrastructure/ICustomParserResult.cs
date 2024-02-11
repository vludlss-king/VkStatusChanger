using CommandLine;

namespace VkStatusChanger.Worker.Contracts.Infrastructure;

public interface ICustomParserResult
{
    Task<ParserResult<object>> WithParsedAsync<T>(Func<T, Task> action);
}
