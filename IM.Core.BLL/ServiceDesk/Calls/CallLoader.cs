using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    internal class CallLoader : 
        ILoadEntity<Guid, Call>,
        ISelfRegisteredService<ILoadEntity<Guid, Call>>
    {
        private readonly IFindEntityByGlobalIdentifier<Call> _finder;

        public CallLoader(IFindEntityByGlobalIdentifier<Call> finder)
        {
            _finder = finder;
        }

        public Task<Call> LoadAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _finder
                .With(x => x.CallService)
                    .ThenWith(x => x.Service)
                        .ThenWith(x => x.Category)
                .With(x => x.CallService)
                    .ThenWith(x => x.ServiceItem)
                        .ThenWith(x => x.Service)
                .With(x => x.CallService)
                    .ThenWith(x => x.ServiceAttendance)
                        .ThenWith(x => x.Service)
                .With(x => x.Aggregate)
                .With(x => x.CallType)
                .With(x => x.IncidentResult)
                .With(x => x.RequestForServiceResult)
                .With(x => x.Priority)
                .With(x => x.Queue)
                .With(x => x.Urgency)
                .With(x => x.Manhours)
                .With(x => x.FormValues)
                    .ThenWith(f => f.Form)
                .With(x => x.FormValues)
                    .ThenWithMany(fv => fv.Values)
                        .ThenWith(f => f.FormField)
                .FindOrRaiseErrorAsync(id, cancellationToken);
        }
    }
}
