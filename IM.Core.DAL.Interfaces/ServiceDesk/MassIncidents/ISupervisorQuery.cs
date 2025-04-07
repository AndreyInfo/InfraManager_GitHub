using System.Linq;

namespace InfraManager.DAL.ServiceDesk.MassIncidents
{
    public interface ISupervisorQuery<T>
    {
        IQueryable<T> Query(User user);
    }
}
