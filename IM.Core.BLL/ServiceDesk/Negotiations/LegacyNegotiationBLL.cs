using AutoMapper;
using Inframanager.BLL.ListView;
using Inframanager.BLL;
using InfraManager.DAL.ServiceDesk.Negotiations;
using InfraManager.DAL;
using Inframanager;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using System;
using InfraManager.DAL.Users;

namespace InfraManager.BLL.ServiceDesk.Negotiations
{
    internal class LegacyNegotiationBLL<TParent> : EditNegotiationBLL<TParent>
    where TParent : class, ICreateNegotiation
    {
        private readonly IFinder<TParent> _parentFinder;
        private readonly IValidateObjectPermissions<Guid, TParent> _parentAccessValidator;

        public LegacyNegotiationBLL(
            IFinder<TParent> parentFinder,
            IValidateObjectPermissions<Guid, TParent> parentAccessValidator,
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
            ILogger<LegacyNegotiationBLL<TParent>> logger,
            IValidatePermissions<TParent> parentPermissionsValidator)
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
            _parentFinder = parentFinder;
            _parentAccessValidator = parentAccessValidator;
        }

        protected override async Task<TParent> FindParentObjectAsync(Guid flowID, Guid parentObjectID, CancellationToken cancellationToken = default)
        {
            var parentObject = await _parentFinder.FindOrRaiseErrorAsync(parentObjectID, cancellationToken);
            Logger.LogTrace($"FlowID: {flowID}. {typeof(TParent).Name} (ID = {parentObjectID}) is found.");

            return parentObject;
        }

        protected async override Task ValidateParentObjectAccessAsync(Guid flowID, Guid parentObjectID, CancellationToken cancellationToken = default)
        {
            await _parentAccessValidator.ValidateObjectAccessOrRaiseErrorAsync(parentObjectID, CurrentUser.UserId, Logger, cancellationToken);
        }
    }
}
