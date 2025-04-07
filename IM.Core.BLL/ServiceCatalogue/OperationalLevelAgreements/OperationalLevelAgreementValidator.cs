using FluentValidation;
using Inframanager.BLL;
using InfraManager.BLL.Localization;
using Inframanager.BLL.Validation;
using InfraManager.DAL.ServiceCatalogue.OperationalLevelAgreements;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.ServiceCatalogue.OperationalLevelAgreements;

public class OperationalLevelAgreementValidator :
    FluentValidator<OperationalLevelAgreement>,
    ISelfRegisteredService<IValidateObject<OperationalLevelAgreement>>
{
    public OperationalLevelAgreementValidator(ILocalizeText localizer) : base(localizer)
    {
    }

    protected override void Initializing()
    {
        RuleFor(x => x.Name).MaximumLength(OperationalLevelAgreement.MaxNameLength).WithMessage(Resources.NameTooLong);
        RuleFor(x => x.Name).NotEmpty().WithMessage(Resources.NameCantBeEmpty);
        RuleFor(x => x.Name).NotNull().WithMessage(Resources.NameCantBeEmpty);
        
        RuleFor(x => x.Number).NotNull().WithMessage(Resources.NumberCantBeNull);
        RuleFor(x => x.Number).MaximumLength(OperationalLevelAgreement.MaxNameLength)
            .WithMessage(Resources.NumberTooLong);
        RuleFor(x => x.Note).MaximumLength(OperationalLevelAgreement.MaxNoteLength).WithMessage(Resources.NoteTooLong);
    }
}