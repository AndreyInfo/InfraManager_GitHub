using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.OrganizationStructure
{
    public interface IOwnerDataProvider
    {
        Task<Owner> GetOwnerAsync(CancellationToken cancellationToken = default);
    }
}
