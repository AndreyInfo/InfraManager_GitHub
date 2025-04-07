using InfraManager.DAL.ChangeTracking;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    internal class CallReferenceVisitor<TReference> : 
        IVisitNewEntity<CallReference<TReference>>, 
        IVisitDeletedEntity<CallReference<TReference>>
        where TReference : IHaveUtcModifiedDate, IGloballyIdentifiedEntity
    {
        private readonly IFinder<Call> _callFinder;
        private readonly IFinder<TReference> _objectFinder;
        public CallReferenceVisitor(IFinder<Call> callFinder, IFinder<TReference> objectFinder)
        {
            _callFinder = callFinder;
            _objectFinder = objectFinder;
        }
        public void Visit(CallReference<TReference> entity)
        {
            var call = _callFinder.Find(entity.CallID);
            var referencedObject = _objectFinder.Find(entity.ObjectID);

            if (call == null || referencedObject == null)
            {
                return;
            }

            call.UtcDateModified = DateTime.UtcNow;
            referencedObject.UtcDateModified = DateTime.UtcNow;            
        }

        public void Visit(IEntityState originalState, CallReference<TReference> entity)
        {
            Visit(entity);
        }

        public async Task VisitAsync(CallReference<TReference> entity, CancellationToken cancellationToken)
        {
            var call = await _callFinder.FindAsync(entity.CallID);
            var referencedObject = await _objectFinder.FindAsync(entity.ObjectID);

            if (call != null && referencedObject != null)
            {
                if (call.EntityStateID != null)
                    call.UtcDateModified = DateTime.UtcNow;
                referencedObject.UtcDateModified = DateTime.UtcNow;
            }
        }

        public async Task VisitAsync(IEntityState originalState, CallReference<TReference> entity, CancellationToken cancellationToken)
        {
            await VisitAsync(entity, cancellationToken);
        }
    }
}
