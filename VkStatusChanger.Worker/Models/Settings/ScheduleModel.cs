﻿namespace VkStatusChanger.Worker.Models.Settings
{
    public class ScheduleModel
    {
        /// <summary>
        /// Расписания смены статусов
        /// </summary>
        public List<ScheduleItem> Items { get; set; } = new();
    }
}
