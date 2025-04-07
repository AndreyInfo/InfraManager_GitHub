using InfraManager.DAL.ServiceDesk.Manhours;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk
{
    [Obsolete("Switch to IAbstractFinder<IHaveManhours, Guid>")]
    public interface IFindEntityWithManhours
    {
        Task<IHaveManhours> FindAsync(Guid id, CancellationToken cancellationToken);
    }
}