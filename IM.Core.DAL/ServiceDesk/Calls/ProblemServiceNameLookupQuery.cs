using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.ServiceDesk.Calls;

internal class ProblemServiceNameLookupQuery : ILookupQuery
{
    private readonly DbSet<Problem> _problems;

    public ProblemServiceNameLookupQuery(DbSet<Problem> problems)
    {
        _problems = problems;
    }

    public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var query = _problems
            .Include(x => x.Service)
            .AsNoTracking()
            .Where(x => x.ServiceID.HasValue)
            .Select(x => new
            {
                ID = x.ServiceID,
                Info = x.Service.Name,
            });
            
        return Array.ConvertAll(
            await query.Distinct().ToArrayAsync(cancellationToken),
            x => new ValueData { ID = x.ID.ToString(), Info = x.Info, });
    }
}