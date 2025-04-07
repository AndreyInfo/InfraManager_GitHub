using InfraManager.DAL;
using InfraManager.DAL.Events;

namespace InfraManager.BLL.WorkFlow
{
    public interface ISendWorkflowEntityEvent<T> where T : IWorkflowEntity
    {
        void Send(T entity, EventType eventType, string targetState = null);
    }
}
