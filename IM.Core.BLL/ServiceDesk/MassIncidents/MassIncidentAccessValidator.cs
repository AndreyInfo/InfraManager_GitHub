using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using InfraManager.DAL.ServiceDesk.Negotiations;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    internal class MassIncidentAccessValidator : SpecificationPermissionsValidator<int, MassIncident>,
        ISelfRegisteredService<IValidateObjectPermissions<int, MassIncident>>
    {
        private readonly IReadonlyRepository<MassIncident> _repository;
        private readonly ServiceDeskObjectAccessIsNotRestricted _accessNotRestricted;

        public MassIncidentAccessValidator(
            IFindEntityByGlobalIdentifier<User> userFinder, 
            IObjectClassProvider<MassIncident> massIncidentObjectClassProvider,
            IReadonlyRepository<MassIncident> repository,
            ServiceDeskObjectAccessIsNotRestricted accessNotRestricted,
            UserIsSupervisor<MassIncident> userIsSupervisor,
            MassIncidentIsAvailableViaToz availableViaToz,
            IBuildUserInNegotiationSpecification<MassIncident> userInNegotiation,
            IObjectClassProvider<MassIncident> classProvider) 
            : base(
                  userFinder,
                  classProvider,
                  SpecificationBuilder<MassIncident, User>.Any(
                      MassIncident.UserIsCreator,
                      MassIncident.UserIsExecutor,
                      MassIncident.UserIsOwner,
                      MassIncident.UserIsInGroup,
                      userIsSupervisor,
                      availableViaToz,
                      userInNegotiation))
        {
            _repository = repository;
            _accessNotRestricted = accessNotRestricted;
        }

        protected override async Task<MassIncident> FindEntityAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _repository
                .With(x => x.CreatedBy)
                .With(x => x.OwnedBy)
                .With(x => x.ExecutedByUser)
                .With(x => x.ExecutedByGroup)
                .ThenWith(x => x.QueueUsers)
                .SingleOrDefaultAsync(x => x.ID == id, cancellationToken);
        }

        protected override IFindEntityByGlobalIdentifier<User> Include(IFindEntityByGlobalIdentifier<User> userFinder)
        {
            return userFinder
                .WithMany(x => x.UserRoles)
                .ThenWith(x => x.Role)
                .ThenWithMany(x => x.Operations);
        }

        protected override bool NotRestricted(User user)
        {
            return _accessNotRestricted.IsSatisfiedBy(user);
        }
    }
}
