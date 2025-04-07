using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ChangeTracking
{
    public interface  IVisitDeletedEntity<T>
    {
        void Visit(IEntityState originalState, T entity);
        Task VisitAsync(IEntityState originalState, T entity, CancellationToken cancellationToken);
    }
}
