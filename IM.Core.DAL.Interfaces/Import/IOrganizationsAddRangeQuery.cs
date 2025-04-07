using System.Collections.Generic;
using InfraManager.DAL.OrganizationStructure;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Import
{
    public interface IOrganizationsAddRangeQuery
    {
        Task ExecuteAsync(IEnumerable<Organization> organizations, CancellationToken cancellationToken);
    }
}
