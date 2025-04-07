using FluentValidation;
using Inframanager.BLL;
using InfraManager.BLL.Localization;
using Inframanager.BLL.Validation;
using InfraManager.DAL;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.CreepingLines
{
    /// <summary>
    /// Проверяет данные CreepingLineDetails
    /// </summary>
    internal class CreepingLineDetailsValidator :
        FluentValidator<CreepingLineData>, 
        ISelfRegisteredService<IValidateObject<CreepingLineData>> 
    {
        public CreepingLineDetailsValidator(ILocalizeText localizer) : base(localizer)
        {
        }

        protected override void Initializing()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage(nameof(Resources.CreepingLine_Name_Required));
            RuleFor(x => x.Name).MaximumLength(CreepingLine.MaxNameLength)
                .WithMessage(nameof(Resources.CreepingLine_Max_Length));
        }
    }
}
