using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ChangeTracking
{
    public interface IVisitNewEntity<T> where T : class
    {
        void Visit(T entity);
        Task VisitAsync(T entity, CancellationToken cancellationToken);
    }
}