﻿using VkStatusChanger.Worker.Enums;

namespace VkStatusChanger.Worker.Models.Settings
{
    internal class SettingsModel
    {
        public EveryModel? Every { get; set; }
        public ScheduleModel? Schedule { get; set; }
        public SettingsType SettingsType { get; set; }
    }
}
