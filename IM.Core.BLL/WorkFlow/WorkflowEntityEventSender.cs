using Inframanager;
using InfraManager.DAL;
using InfraManager.DAL.Events;

namespace InfraManager.BLL.WorkFlow
{
    internal class WorkflowEntityEventSender<T> : ISendWorkflowEntityEvent<T>
        where T : IWorkflowEntity
    {
        private readonly IRepository<EntityEvent> _entityEvents;
        private readonly IRepository<ExternalEventReference> _externalEventReferences;
        private readonly IObjectClassProvider<T> _objectClassProvider;
        private readonly ICurrentUser _currentUser;

        public WorkflowEntityEventSender(
            IRepository<EntityEvent> entityEvents, 
            IRepository<ExternalEventReference> externalEventReferences, 
            IObjectClassProvider<T> objectClassProvider,
            ICurrentUser currentUser)
        {
            _entityEvents = entityEvents;
            _externalEventReferences = externalEventReferences;
            _objectClassProvider = objectClassProvider;
            _currentUser = currentUser;
        }

        public void Send(T entity, EventType eventType, string targetState = null)
        {
            var entityEvent = new EntityEvent(
                EventSource.Application,
                eventType,
                _objectClassProvider.GetObjectClass(),
                entity.IMObjID,
                _currentUser.UserId,
                targetState);
            _entityEvents.Insert(entityEvent);

            if (!string.IsNullOrWhiteSpace(entity.WorkflowSchemeIdentifier) && entity.WorkflowSchemeID.HasValue)
            {
                _externalEventReferences.Insert(
                    new ExternalEventReference(entityEvent.Id, entity.IMObjID));
            }
        }
    }
}
