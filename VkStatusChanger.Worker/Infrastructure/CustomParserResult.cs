﻿using CommandLine;
using VkStatusChanger.Worker.Contracts.Infrastructure;

namespace VkStatusChanger.Worker.Infrastructure
{
    public class CustomParserResult : ICustomParserResult
    {
        private readonly ParserResult<object?> _parserResult;

        public CustomParserResult(ParserResult<object?> parserResult)
        {
            _parserResult = parserResult;    
        }

        public ParserResult<object?> WithParsed<T>(Action<T> action)
            => _parserResult.WithParsed(action);

        public async Task<ParserResult<object>> WithParsedAsync<T>(Func<T, Task> action)
            => await _parserResult.WithParsedAsync(action);
    }
}
