using VkNet.Abstractions;
using VkStatusChanger.Worker.Contracts.Infrastructure;

namespace VkStatusChanger.Worker.Infrastructure.HttpClients
{
    internal class VkStatusHttpClient : IVkStatusHttpClient
    {
        private readonly IVkApi _vkApi;

        public VkStatusHttpClient(IVkApi vkApi)
        {
            _vkApi = vkApi;    
        }

        public async Task<bool> SetStatus(string text)
            => await _vkApi.Status.SetAsync(text);
    }
}
