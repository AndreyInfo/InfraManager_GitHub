using InfraManager.DAL;
using InfraManager.DAL.ChangeTracking;
using InfraManager.DAL.Events;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.WorkFlow
{
    internal class WorkflowEntityCreatedEventVisitor<T> : IVisitNewEntity<T>
        where T : class, IWorkflowEntity
    {
        ISendWorkflowEntityEvent<T> _eventSender;

        public WorkflowEntityCreatedEventVisitor(ISendWorkflowEntityEvent<T> eventSender)
        {
            _eventSender = eventSender;
        }

        public void Visit(T entity)
        {
            _eventSender.Send(entity, EventType.EntityCreated);
        }

        public Task VisitAsync(T entity, CancellationToken cancellationToken)
        {
            Visit(entity);
            return Task.CompletedTask;
        }
    }
}
