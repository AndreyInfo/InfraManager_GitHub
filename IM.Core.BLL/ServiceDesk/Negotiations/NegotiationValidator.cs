using FluentValidation;
using Inframanager.BLL;
using Inframanager.BLL.Validation;
using InfraManager.BLL.Localization;
using InfraManager.DAL.ServiceDesk.Negotiations;

namespace InfraManager.BLL.ServiceDesk.Negotiations
{
    internal class NegotiationValidator :
        FluentValidator<Negotiation>,
        ISelfRegisteredService<IValidateObject<Negotiation>>
    {
        public NegotiationValidator(ILocalizeText localizer) 
            : base(localizer)
        {
        }

        protected override void Initializing()
        {
            RuleFor(x => x.Name)
                .MaximumLength(Negotiation.NameMaxLength)
                .WithMessage("Negotiation_NameTooLong");
        }
    }
}
