using InfraManager.BLL.WorkFlow;
using InfraManager.DAL;
using InfraManager.DAL.ChangeTracking;
using InfraManager.DAL.Events;
using InfraManager.DAL.ServiceDesk;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    internal class CallWorkflowEntityEventVisitor : IVisitModifiedEntity<Call>, ISelfRegisteredService<IVisitModifiedEntity<Call>>
    {
        private readonly ISendWorkflowEntityEvent<Call> _eventSender;
        private readonly ICurrentUser _currentUser;

        public CallWorkflowEntityEventVisitor(ISendWorkflowEntityEvent<Call> eventSender, ICurrentUser currentUser)
        {
            _eventSender = eventSender;
            _currentUser = currentUser;
        }

        public void Visit(IEntityState originalState, Call currentState)
        {
            if (currentState.Removed)
            {
                _eventSender.Send(currentState, EventType.EntityDeleted);
            }
            else if (_currentUser.UserId != User.SystemUserGlobalIdentifier)
            {
                _eventSender.Send(currentState, currentState.Removed ? EventType.EntityDeleted : EventType.EntityModified);
            }
        }

        public Task VisitAsync(IEntityState originalState, Call currentState, CancellationToken cancellationToken)
        {
            Visit(originalState, currentState);
            return Task.CompletedTask;
        }
    }
}
