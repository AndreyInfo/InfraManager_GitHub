using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    internal class CallTypeLoader : 
        ILoadEntity<Guid, CallType>,
        ISelfRegisteredService<ILoadEntity<Guid, CallType>>,
        IBuildEntityQuery<CallType, CallTypeDetails, CallTypeListFilter>,
        ISelfRegisteredService<IBuildEntityQuery<CallType, CallTypeDetails, CallTypeListFilter>>
    {
        private IReadonlyRepository<CallType> _repository;

        public CallTypeLoader(IReadonlyRepository<CallType> repository)
        {
            _repository = repository;
        }

        public async Task<CallType> LoadAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var allCallTypes = await AllCallTypes(cancellationToken); // нужна вся иерархия
            return allCallTypes.SingleOrDefault(x => x.ID == id);
        }

        public IExecutableQuery<CallType> Query(CallTypeListFilter filterBy)
        {
            var query = _repository.With(x => x.Parent).ThenWith(x => x.Parent).Query(); // TODO: Убрать костыль, просто загружать все без ограничений на глубину вложенности

            if (filterBy.VisibleInWeb.HasValue)
            {
                query = query.Where(x => x.VisibleInWeb == filterBy.VisibleInWeb);
            }

            return query;
        }

        private Task<CallType[]> AllCallTypes(CancellationToken cancellationToken = default) =>
            _repository.With(x => x.Parent).ToArrayAsync(cancellationToken);
    }
}
