using InfraManager.DAL;
using InfraManager.DAL.ChangeTracking;
using InfraManager.DAL.Documents;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.WorkOrders;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    internal class CallAggregateUpdater : 
        DocumentCountAggregateUpdater<Call>,
        IVisitModifiedEntity<WorkOrder>,
        ISelfRegisteredService<IVisitModifiedEntity<WorkOrder>>,
        IVisitNewEntity<WorkOrder>,
        ISelfRegisteredService<IVisitNewEntity<WorkOrder>>,
        IVisitNewEntity<CallReference<Problem>>,
        ISelfRegisteredService<IVisitNewEntity<CallReference<Problem>>>,
        IVisitDeletedEntity<CallReference<Problem>>,
        ISelfRegisteredService<IVisitDeletedEntity<CallReference<Problem>>>,
        ISelfRegisteredService<IVisitNewEntity<DocumentReference>>,
        ISelfRegisteredService<IVisitDeletedEntity<DocumentReference>>
    {
        #region .ctor

        private readonly IFinder<WorkOrderReference> _referenceFinder;

        public CallAggregateUpdater(
            IFinder<Call> callFinder,
            IFinder<WorkOrderReference> referenceFinder) 
            : base(ObjectClass.Call, callFinder.With(x => x.Aggregate))
        {
            _referenceFinder = referenceFinder;
        }

        #endregion

        #region syncronous

        public void Visit(IEntityState originalState, WorkOrder currentState)
        {
            var previousReferenceID = (long)originalState[nameof(WorkOrder.WorkOrderReferenceID)];
            if (currentState.WorkOrderReferenceID != previousReferenceID)
            {
                var previousReference = _referenceFinder.Find(previousReferenceID);                
                DecrementWorkOrderCountIfNeeded(previousReference);

                var currentReference = _referenceFinder.Find(currentState.WorkOrderReferenceID);
                IncrementWorkOrderCountIfNeeded(currentReference);
            }
        }

        public void Visit(WorkOrder entity)
        {
            var reference = _referenceFinder.Find(entity.WorkOrderReferenceID);
            IncrementWorkOrderCountIfNeeded(reference);
        }

        private void IncrementWorkOrderCountIfNeeded(WorkOrderReference reference) =>
            UpdateWorkOrderCountIfNeeded(reference, x => x + 1);

        private void DecrementWorkOrderCountIfNeeded(WorkOrderReference reference) => 
            UpdateWorkOrderCountIfNeeded(reference, x => x - 1);

        private void UpdateWorkOrderCountIfNeeded(WorkOrderReference reference, Func<int, int> func)
        {
            if (reference.ObjectClassID == ObjectClass.Call)
            {
                var call = Finder.Find(reference.ObjectID);
                call.Aggregate.WorkOrderCount = func(call.Aggregate.WorkOrderCount);
            }
        }

        #endregion

        #region asyncronous

        public async Task VisitAsync(IEntityState originalState, WorkOrder currentState, CancellationToken cancellationToken)
        {
            var previousReferenceID = (long)originalState[nameof(WorkOrder.WorkOrderReferenceID)];
            if (currentState.WorkOrderReferenceID != previousReferenceID)
            {
                var previousReference = await _referenceFinder.FindAsync(previousReferenceID, cancellationToken);
                await DecrementWorkOrderCountIfNeededAsync(previousReference, cancellationToken);

                var currentReference = await _referenceFinder.FindAsync(currentState.WorkOrderReferenceID, cancellationToken);
                await IncrementWorkOrderCountIfNeededAsync(currentReference, cancellationToken);
            }
        }

        public async Task VisitAsync(WorkOrder entity, CancellationToken cancellationToken)
        {
            var reference = await _referenceFinder.FindAsync(entity.WorkOrderReferenceID, cancellationToken);
            if (reference.ObjectClassID == ObjectClass.Call)
            {
                await IncrementWorkOrderCountIfNeededAsync(reference, cancellationToken);
            }
        }

        private async Task IncrementWorkOrderCountIfNeededAsync(WorkOrderReference reference, CancellationToken cancellationToken = default) =>
            await UpdateWorkOrderCountIfNeededAsync(reference, x => x + 1, cancellationToken);

        private async Task DecrementWorkOrderCountIfNeededAsync(WorkOrderReference reference, CancellationToken cancellationToken = default) =>
            await UpdateWorkOrderCountIfNeededAsync(reference, x => x - 1, cancellationToken);

        private async Task UpdateWorkOrderCountIfNeededAsync(WorkOrderReference reference, Func<int, int> func, CancellationToken cancellationToken = default)
        {
            if (reference.ObjectClassID == ObjectClass.Call)
            {
                var call = await Finder.FindAsync(reference.ObjectID, cancellationToken);
                call.Aggregate.WorkOrderCount = func(call.Aggregate.WorkOrderCount);
            }
        }

        #endregion

        #region ProblemCount

        void IVisitNewEntity<CallReference<Problem>>.Visit(CallReference<Problem> entity)
            => IncrementProblemCount(entity.CallID);

        async Task IVisitNewEntity<CallReference<Problem>>.VisitAsync(CallReference<Problem> entity, CancellationToken cancellationToken)
            => await IncrementProblemCountAsync(entity.CallID, cancellationToken);

        void IVisitDeletedEntity<CallReference<Problem>>.Visit(IEntityState originalState, CallReference<Problem> entity)
            => DecrementProblemCount(entity.CallID);

        async Task IVisitDeletedEntity<CallReference<Problem>>.VisitAsync(IEntityState originalState, CallReference<Problem> entity, CancellationToken cancellationToken)
            => await DecrementProblemCountAsync(entity.CallID, cancellationToken);

        private void IncrementProblemCount(Guid callID)
            => UpdateProblemCount(callID, count => count + 1);
        
        private async Task IncrementProblemCountAsync(Guid callID, CancellationToken cancellationToken)
            => await UpdateProblemCountAsync(callID, count => count + 1, cancellationToken);
        
        private void DecrementProblemCount(Guid callID)
            => UpdateProblemCount(callID, count => count - 1);
        
        private async Task DecrementProblemCountAsync(Guid callID, CancellationToken cancellationToken)
            => await UpdateProblemCountAsync(callID, count => count - 1, cancellationToken);
        
        private void UpdateProblemCount(Guid callID, Func<int, int> updateFunc)
        {
            var call = Finder.Find(callID);
            call.Aggregate.ProblemCount = updateFunc(call.Aggregate.ProblemCount);
        }

        private async Task UpdateProblemCountAsync(Guid callID, Func<int, int> updateFunc, CancellationToken cancellationToken)
        {
            var call = await Finder.FindAsync(callID, cancellationToken);
            call.Aggregate.ProblemCount = updateFunc(call.Aggregate.ProblemCount);
        }

        #endregion

        #region DocumentCount

        protected override void UpdateDocumentCount(Call entity, Func<int, int> func)
        {
            entity.Aggregate.DocumentCount = func(entity.Aggregate.DocumentCount);
        }

        #endregion
    }
}
