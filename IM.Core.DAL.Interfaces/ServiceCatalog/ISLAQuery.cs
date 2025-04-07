using System.Threading.Tasks;
using System.Threading;
using System;
using InfraManager.DAL.ServiceDesk.Calls;

namespace InfraManager.DAL.ServiceCatalog
{
    public interface ISLAQuery
    {
        Task<SLAItem[]> GetByUserAsync(Guid userId, CancellationToken cancellationToken);

        Task<SLAReferenceItem[]> GetReferencesAsync(CancellationToken cancellationToken);
    }
}
