using FluentValidation;
using Inframanager.BLL;
using InfraManager.BLL.Localization;
using Inframanager.BLL.Validation;
using InfraManager.DAL.ServiceDesk;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.ServiceDesk.KnowledgeBase.KnowledgeBaseClassifiers;

public class KnowledgeBaseClassifierValidator : 
    FluentValidator<KBArticleFolder>,
    ISelfRegisteredService<IValidateObject<KBArticleFolder>>
{
    public KnowledgeBaseClassifierValidator(ILocalizeText localizer)
        : base(localizer)
    {
    }

    protected override void Initializing()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleFor(x => x.Name).NotEmpty()
            .WithMessage(nameof(Resources.NameCantBeEmpty));
        
        RuleFor(x => x.Name).NotNull()
            .WithMessage(nameof(Resources.NameCantBeEmpty));
        
        RuleFor(x => x.Note).NotNull().WithMessage(nameof(Resources.NoteCantBeEmpty));
        RuleFor(x => x.Name).MaximumLength(KBArticleFolder.MaxNameLength).WithMessage(nameof(Resources.NameTooLong));
        RuleFor(x => x.Note).MaximumLength(KBArticleFolder.MaxNoteLength).WithMessage(nameof(Resources.NoteTooLong));
    }
}