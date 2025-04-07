using InfraManager.DAL;
using InfraManager.DAL.ChangeTracking;
using InfraManager.DAL.Events;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.WorkFlow
{
    internal class WorkflowSetStateEventVisitor<T> : IVisitNewEntity<T>, IVisitModifiedEntity<T>
        where T : class, IWorkflowEntity
    {
        private readonly ISendWorkflowEntityEvent<T> _eventSender;

        public WorkflowSetStateEventVisitor(ISendWorkflowEntityEvent<T> eventSender)
        {
            _eventSender = eventSender;
        }

        public void Visit(T entity)
        {
            SendSetStateIfNeeded(entity);
        }

        public void Visit(IEntityState originalState, T currentState)
        {
            SendSetStateIfNeeded(currentState);
        }

        private void SendSetStateIfNeeded(T entity)
        {
            if (!string.IsNullOrWhiteSpace(entity.TargetEntityStateID))
            {
                _eventSender.Send(entity, EventType.EntityStateSet, entity.TargetEntityStateID);
                entity.TargetEntityStateID = null;
            }
        }

        public Task VisitAsync(T entity, CancellationToken cancellationToken)
        {
            Visit(entity);
            return Task.CompletedTask;
        }

        public Task VisitAsync(IEntityState originalState, T currentState, CancellationToken cancellationToken)
        {
            Visit(originalState, currentState);
            return Task.CompletedTask;
        }
    }
}
