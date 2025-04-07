using FluentValidation;
using Inframanager.BLL;
using Inframanager.BLL.Validation;
using InfraManager.BLL.Localization;

namespace InfraManager.BLL.OrganizationStructure
{
    internal class DeputyUserDataValidator :
        FluentValidator<DeputyUserData>, // наследуем базовый валидатор на основе FluentValidation
        ISelfRegisteredService<IValidateObject<DeputyUserData>> // саморегистрируемся как валидатор объекта CallData
    {
        public DeputyUserDataValidator(ILocalizeText localizer) : base(localizer)
        {
        }

        protected override void Initializing()
        {
            RuleFor(x => x.UtcDeputySince).LessThan(x => x.UtcDeputyUntil).WithMessage("DeputyUser_EndDateMustBeGraterThanStartDate");
        }
    }
}