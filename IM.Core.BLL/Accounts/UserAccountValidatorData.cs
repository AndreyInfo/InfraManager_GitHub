using FluentValidation;
using Inframanager.BLL;
using InfraManager.BLL.Localization;
using Inframanager.BLL.Validation;
using InfraManager.DAL.Accounts;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.Accounts;

public class UserAccountDataValidator : FluentValidator<UserAccountData>,
    ISelfRegisteredService<IValidateObject<UserAccountData>>
{
    private readonly int MaxLength = 50;
    private readonly ILocalizeText _localizer;

    public UserAccountDataValidator(ILocalizeText localizer) : base(localizer)
    {
        _localizer = localizer;
    }

    protected override void Initializing()
    {
        RuleFor(x => x.Login).Cascade(CascadeMode.Stop).MaximumLength(MaxLength)
            .WithMessage(
                string.Format(_localizer.Localize(string.Format(nameof(Resources.String_Cant_Be_More_Than_X_Length))),
                    MaxLength));
        
        RuleFor(x => x.Name).Cascade(CascadeMode.Stop).MaximumLength(MaxLength)
            .WithMessage(
                string.Format(_localizer.Localize(string.Format(nameof(Resources.String_Cant_Be_More_Than_X_Length))),
                    MaxLength));
        
        RuleFor(x => x.Password).Cascade(CascadeMode.Stop).MaximumLength(MaxLength)
            .WithMessage(
                string.Format(_localizer.Localize(string.Format(nameof(Resources.String_Cant_Be_More_Than_X_Length))),
                    MaxLength));
        
        RuleFor(x => x.PrivacyKey).Cascade(CascadeMode.Stop).MaximumLength(MaxLength)
            .WithMessage(
                string.Format(_localizer.Localize(string.Format(nameof(Resources.String_Cant_Be_More_Than_X_Length))),
                    MaxLength));
        
        RuleFor(x => x.SSH_Passphrase).Cascade(CascadeMode.Stop).MaximumLength(MaxLength)
            .WithMessage(
                string.Format(_localizer.Localize(string.Format(nameof(Resources.String_Cant_Be_More_Than_X_Length))),
                    MaxLength));

        RuleFor(x => x.SSH_PrivateKey).Cascade(CascadeMode.Stop).MaximumLength(MaxLength)
            .WithMessage(
                string.Format(_localizer.Localize(string.Format(nameof(Resources.String_Cant_Be_More_Than_X_Length))),
                    MaxLength));
    }
}