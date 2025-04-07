using System;

namespace InfraManager.DAL.Events
{
    public interface IEventRepository : IRepository<Event>
    {
        void WhenAdded(InframanagerObject relatedObject, Action<Event> onEventAdded);
    }
}
