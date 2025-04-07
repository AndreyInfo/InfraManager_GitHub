using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL;

internal class BulkDelete<TEntity> : IBulkDelete<TEntity>
    where TEntity : class, IImportEntity, new()
{
    private readonly DbContext _context;

    public BulkDelete(CrossPlatformDbContext context)
    {
        _context = context;
    }

    public Task DeleteExternalIDAsync(IEnumerable<string> keys, CancellationToken token)
    {
        var entities = keys.Select(x => new TEntity()
        {
            ExternalID = x
        }).ToList();
        return _context.BulkDeleteAsync(entities, cancellationToken: token);
    }
}