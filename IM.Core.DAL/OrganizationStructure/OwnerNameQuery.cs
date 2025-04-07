using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.OrganizationStructure;
internal sealed class OwnerNameQuery : IOwnerNameQuery, ISelfRegisteredService<IOwnerNameQuery>
{
    private readonly IOwnerQuery _query;

    public OwnerNameQuery(IOwnerQuery query)
    {
        _query = query;
    }

    public async Task<string> ExecuteAsync(Guid id, CancellationToken cancellationToken)
        => (await _query.Query().FirstOrDefaultAsync(x => x.ID == id, cancellationToken))?.Name;
}
