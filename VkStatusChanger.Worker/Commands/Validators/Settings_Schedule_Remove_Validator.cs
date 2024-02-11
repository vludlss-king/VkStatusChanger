using FluentValidation;

namespace VkStatusChanger.Worker.Commands.Validators
{
    internal class Settings_Schedule_Remove_Validator : AbstractValidator<Routes.Settings.Schedule.Remove>
    {
        public Settings_Schedule_Remove_Validator()
        {
            
        }
    }
}
