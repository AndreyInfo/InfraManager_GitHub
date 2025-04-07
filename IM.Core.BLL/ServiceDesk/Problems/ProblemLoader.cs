using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Problems
{
    internal class ProblemLoader :
        ILoadEntity<Guid, Problem>,
        ISelfRegisteredService<ILoadEntity<Guid, Problem>>
    {
        private readonly IFindEntityByGlobalIdentifier<Problem> _finder;

        public ProblemLoader(IFindEntityByGlobalIdentifier<Problem> finder)
        {
            _finder = finder;
        }

        public Task<Problem> LoadAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _finder
                .With(x => x.Urgency)
                .With(x => x.Influence)
                .With(x => x.Type)
                .With(x => x.Priority)
                .With(x => x.ProblemCause)
                .With(x => x.Dependencies)
                .With(x => x.Negotiations)
                .With(x => x.CallReferences)
                .With(x => x.Notes)
                .With(x => x.Manhours)
                .With(x => x.WorkOrderReferences)
                .FindOrRaiseErrorAsync(id, cancellationToken);
        }
    }
}
