using FluentValidation;

namespace VkStatusChanger.Worker.Commands.Validators
{
    internal class Settings_Every_Set_Validator : AbstractValidator<Routes.Settings.Every.Set>
    {
        public Settings_Every_Set_Validator()
        {

        }
    }
}
