using Inframanager.BLL.Events;
using InfraManager.DAL;
using InfraManager.DAL.ChangeTracking;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk
{
    internal class CallReferenceSubjectFinder<TReference> : ISubjectFinder<CallReference<TReference>, TReference>
        where TReference : IHaveUtcModifiedDate, IGloballyIdentifiedEntity
    {
        private readonly IFinder<TReference> _finder;

        public CallReferenceSubjectFinder(IFinder<TReference> finder)
        {
            _finder = finder;
        }

        public TReference Find(CallReference<TReference> entity, IEntityState originalState)
        {
            return _finder.Find(entity.ObjectID);
        }
    }
}
