using FluentValidation;
using Inframanager.BLL;
using InfraManager.BLL.Localization;
using Inframanager.BLL.Validation;

namespace InfraManager.BLL.ServiceDesk;

internal class ExecutorListFilterValidator :
    FluentValidator<ExecutorListFilter>,
    ISelfRegisteredService<IValidateObject<ExecutorListFilter>>
{
    public ExecutorListFilterValidator(ILocalizeText localizer)
        : base(localizer)
    {
        RuleFor(x => x.SDExecutor)
            .Must((filter, _) => !filter.SDExecutor || filter.HasAccessToObjectID.HasValue && filter.HasAccessToObjectClassID.HasValue)
            .WithMessage("HasAccessToObjectID and HasAccessToObjectClassID is required.");
    }

    protected override void Initializing()
    {
    }
}