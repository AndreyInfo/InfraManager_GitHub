using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.OrganizationStructure
{
    public interface ISubdivisionFullNameQuery
    {
        Task<string> QueryAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
