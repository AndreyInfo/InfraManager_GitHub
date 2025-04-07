using InfraManager.DAL.OrganizationStructure;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Import
{
    public interface ISubdivisionsAddRangeQuery
    {
        public Task ExecuteAsync(Subdivision[] subdivisions, CancellationToken cancellationToken);
    }
}
