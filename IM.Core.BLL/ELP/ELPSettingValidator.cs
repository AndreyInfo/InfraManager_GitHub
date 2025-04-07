using FluentValidation;
using Inframanager.BLL;
using InfraManager.BLL.Localization;
using Inframanager.BLL.Validation;
using InfraManager.DAL.Asset;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.ELP;

internal class ELPSettingValidator : 
    FluentValidator<ElpSetting>,
    ISelfRegisteredService<IValidateObject<ElpSetting>>
{
    private readonly ILocalizeText _localizer;

    public ELPSettingValidator(ILocalizeText localizer) : base(localizer)
    {
        _localizer = localizer;
    }

    protected override void Initializing()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage(_localizer.Localize(nameof(Resources.ELP_Setting_Name_Required)));
    }
}