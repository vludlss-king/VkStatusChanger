namespace VkStatusChanger.Worker.Contracts.Infrastructure
{
    internal interface IVkStatusHttpClient
    {
        Task<bool> SetStatus(string text);
    }
}
