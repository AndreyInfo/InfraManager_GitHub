using FluentValidation;
using Inframanager.BLL;
using Inframanager.BLL.Validation;
using InfraManager.BLL.Localization;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using InfraManager.ResourcesArea;
using System.Linq;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    internal class MassIncidentValidator : FluentValidator<MassIncident>,
        ISelfRegisteredService<IValidateObject<MassIncident>>
    {
        private readonly IReadonlyRepository<TechnicalFailureCategory> _technicalCategoryFailuresRepository;

        public MassIncidentValidator(
            IReadonlyRepository<TechnicalFailureCategory> technicalCategoryFailuresRepository,
            ILocalizeText localizer) 
            : base(localizer)
        {
            _technicalCategoryFailuresRepository = technicalCategoryFailuresRepository;
        }

        protected override void Initializing()
        {
            RuleFor(massIncident => massIncident.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage(nameof(Resources.MassIncident_EmptyName));
            RuleFor(massIncident => massIncident.TechnicalFailureCategoryID)
                .Must((massIncident, categoryID) =>
                {
                    if (!categoryID.HasValue)
                    {
                        return true;
                    }

                    var category = _technicalCategoryFailuresRepository
                        .WithMany(x => x.Services)
                        .ThenWith(x => x.Reference)
                        .FirstOrDefault(x => x.ID == categoryID);

                    return category == null
                        || TechnicalFailureCategory.AvailableForService.Build(massIncident.ServiceID).IsSatisfiedBy(category);
                })
                .WithMessage(nameof(Resources.MassIncident_TechnicalFailureCategoryNotAvailableForService));
              RuleFor(massIncident => massIncident.ExecutedByUser)
                .Must((massIndent, executor) =>
                    executor.IMObjID == User.NullUserGloablIdentifier
                        || massIndent.ExecutedByGroupID == Group.NullGroupID
                        || MassIncident.UserIsInGroup.Build(executor).IsSatisfiedBy(massIndent))
                .WithMessage(nameof(Resources.MassIncident_ExecutorNotInGroup));
        }
    }
}
