using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.BLL.ServiceDesk.Negotiations;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using InfraManager.DAL.ServiceDesk.Negotiations;
using InfraManager.DAL.Users;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    internal class MassIncidentNegotiationBLL : EditNegotiationBLL<MassIncident>
    {
        private readonly IFindEntityByGlobalIdentifier<MassIncident> _massIncidentFinder;
        private readonly IValidateObjectPermissions<int, MassIncident> _massIncidentAccessValidator;

        public MassIncidentNegotiationBLL(
            IFindEntityByGlobalIdentifier<MassIncident> massIncidentFinder,
            IValidateObjectPermissions<int, MassIncident> massIncidentAccessValidator,
            IRepository<Negotiation> repository,
            IFinder<Negotiation> finder,
            IUnitOfWork unitOfWork,
            IBuildObject<NegotiationDetails, Negotiation> detailsBuilder,
            IServiceMapper<NegotiationMode, ICalculateNegotiationStatus> statusCalculators,
            IMapper mapper,
            IReadonlyRepository<User> usersRepository,
            IValidateObjectPermissions<Guid, Negotiation> permissionsValidator,
            IRemoveEntityBLL<Guid, Negotiation> removeService,
            IBuildUserIsDeputySpecification userIsDeputySpecificationBuilder,
            ICurrentUser currentUser,
            ILogger<MassIncidentNegotiationBLL> logger,
            IValidatePermissions<MassIncident> parentPermissionsValidator) 
            : base(
                  repository,
                  finder,
                  unitOfWork,
                  detailsBuilder,
                  statusCalculators,
                  mapper,
                  usersRepository,
                  permissionsValidator,
                  removeService,
                  userIsDeputySpecificationBuilder,
                  currentUser,
                  logger,
                  parentPermissionsValidator)
        {
            _massIncidentFinder = massIncidentFinder;
            _massIncidentAccessValidator = massIncidentAccessValidator;
        }

        protected override async Task<MassIncident> FindParentObjectAsync(Guid flowID, Guid parentObjectID, CancellationToken cancellationToken = default)
        {
            var massIncident = await _massIncidentFinder.FindOrRaiseErrorAsync(parentObjectID, cancellationToken);
            Logger.LogTrace($"FlowID: {flowID}. Mass incident (IMObjID = {parentObjectID}) is found.");

            return massIncident;
        }

        protected override async Task ValidateParentObjectAccessAsync(Guid flowID, Guid parentObjectID, CancellationToken cancellationToken = default)
        {
            var massIncident = await _massIncidentFinder.FindAsync(parentObjectID, cancellationToken);

            if (massIncident == null 
                || !await _massIncidentAccessValidator.ObjectIsAvailableAsync(CurrentUser.UserId, massIncident.ID, cancellationToken))
            {
                throw new AccessDeniedException($"Mass incident (IMObjID = {parentObjectID})");
            }
            Logger.LogTrace($"FlowID: {flowID}. Access to mass incident is granted.");
        }
    }
}
