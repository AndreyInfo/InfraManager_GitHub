using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.WorkFlow;
using InfraManager.DAL;
using InfraManager.DAL.ChangeTracking;
using InfraManager.DAL.Events;

namespace InfraManager.BLL.Workflow
{
    // TODO: Реализовать более гибкое решение (по аналогии с Event)
    // либо вынести создание EntityEvent и ExternalEventReference в отдельные BLL
    public class WorkflowEntityModifiedEventVisitor<T> : IVisitModifiedEntity<T>
        where T : class, IWorkflowEntity
    {
        private readonly ISendWorkflowEntityEvent<T> _eventSender;
        private readonly ICurrentUser _currentUser;

        public WorkflowEntityModifiedEventVisitor(ISendWorkflowEntityEvent<T> eventSender, ICurrentUser currentUser)
        {
            _currentUser = currentUser;
            _eventSender = eventSender;
        }

        public void Visit(IEntityState originalState, T currentState)
        {
            if (_currentUser.UserId != User.SystemUserGlobalIdentifier // игнорируем изменения порожденные самим WE
                && originalState[nameof(IWorkflowEntity.WorkflowSchemeIdentifier)] != null) // игнорируем изменения, в которых задается сама схема
            {
                _eventSender.Send(currentState, EventType.EntityModified);
            }
        }

        public Task VisitAsync(IEntityState originalState, T currentState, CancellationToken cancellationToken)
        {
            Visit(originalState, currentState);
            return Task.CompletedTask;
        }

    }
}
