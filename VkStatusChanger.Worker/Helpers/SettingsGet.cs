using VkStatusChanger.Worker.Contracts.Helpers;

namespace VkStatusChanger.Worker.Helpers
{
    internal class SettingsGet : ISettingsGet
    {
        public string? SettingsFile { get; set; }
    }
}
