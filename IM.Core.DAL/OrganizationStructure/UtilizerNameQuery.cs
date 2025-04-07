using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.OrganizationStructure;
internal sealed class UtilizerNameQuery : IUtilizerNameQuery, ISelfRegisteredService<IUtilizerNameQuery>
{
    private readonly IUtilizerQuery _query;

    public UtilizerNameQuery(IUtilizerQuery query)
    {
        _query = query;
    }

    public async Task<string> ExecuteAsync(Guid id, CancellationToken cancellationToken)
        => (await _query.Query().FirstOrDefaultAsync(x => x.ID == id, cancellationToken))?.Name;
}
