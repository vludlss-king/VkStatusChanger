namespace VkStatusChanger.Worker.Models.UserSettings;

public class EverySecondsSchedule
{
    /// <summary>
    /// Статусы
    /// </summary>
    public List<string> Statuses { get; set; } = new();
    /// <summary>
    /// Через сколько секунд менять статус
    /// </summary>
    public int Seconds { get; set; }
}
