using FluentValidation;
using Inframanager.BLL;
using InfraManager.BLL.Localization;
using Inframanager.BLL.Validation;
using InfraManager.CrossPlatform.WebApi.Contracts.ELP;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.ELP
{
    internal class ELPItemValidator : 
        FluentValidator<ELPItem>,
        ISelfRegisteredService<IValidateObject<ELPItem>>
    {
        private readonly ILocalizeText _localizer;

        public ELPItemValidator(ILocalizeText localizer) : base(localizer)
        {
            _localizer = localizer;
        }

        protected override void Initializing()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(_localizer.Localize(nameof(Resources.ELP_Setting_Name_Required)));
        }
    }
}