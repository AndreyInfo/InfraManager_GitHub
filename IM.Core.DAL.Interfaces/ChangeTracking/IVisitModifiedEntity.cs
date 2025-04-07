using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ChangeTracking
{
    public interface IVisitModifiedEntity<in T> where T : class
    {
        void Visit(IEntityState originalState, T currentState);
        Task VisitAsync(IEntityState originalState, T currentState, CancellationToken cancellationToken);
    }
}