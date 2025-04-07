using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager.BLL.AccessManagement;
using InfraManager.BLL.Settings;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.Negotiations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Problems
{
    internal class ProblemAccessValidator : SpecificationPermissionsValidator<Guid, Problem>,
        ISelfRegisteredService<IValidateObjectPermissions<Guid, Problem>>
    {
        private IFindEntityByGlobalIdentifier<Problem> _problemFinder;
        private readonly ServiceDeskObjectAccessIsNotRestricted _accessNotRestricted;

        public ProblemAccessValidator(
            IFindEntityByGlobalIdentifier<User> userFinder,  
            IObjectClassProvider<Problem> classProvider,
            IFindEntityByGlobalIdentifier<Problem> problemFinder,
            ServiceDeskObjectAccessIsNotRestricted accessNotRestricted,
            UserIsSupervisor<Problem> userIsSupervisor, // Видеть проблемы ИТ-сотрудников
            NotOwnedProblem notOwnedProblem,
            IBuildUserInNegotiationSpecification<Problem> userInNegotiation) 
            : base(
                  userFinder, 
                  classProvider,
                  SpecificationBuilder<Problem, User>.Any(
                    Problem.UserIsOwner,
                    Problem.UserIsInitiator,
                    Problem.UserIsExecutor,
                    Problem.UserIsInGroup,
                    notOwnedProblem,
                    userIsSupervisor,
                    userInNegotiation))
        {
            _problemFinder = problemFinder;
            _accessNotRestricted = accessNotRestricted;
        }

        protected override async Task<Problem> FindEntityAsync(Guid key, CancellationToken cancellationToken = default)
        {
            return await _problemFinder
                .With(p => p.Initiator)
                .With(p => p.Executor)
                .With(p => p.Owner)
                .With(p => p.Queue)
                    .ThenWithMany(g => g.QueueUsers)
                .FindAsync(key, cancellationToken);
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
