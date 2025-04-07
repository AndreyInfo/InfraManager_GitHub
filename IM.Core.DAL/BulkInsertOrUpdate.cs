using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL;

internal class BulkInsertOrUpdate<TEntity> : IBulkInsertOrUpdate<TEntity> where TEntity : class, IImportEntity
{
    private readonly DbContext _context;

    public BulkInsertOrUpdate(
        CrossPlatformDbContext context
        )
    {
        _context = context;
    }

    public Task ExecuteAsync(IEnumerable<TEntity> data, CancellationToken token)
    {
        var config = new BulkConfig
        {
            UpdateByProperties = new List<string>
            {
                nameof(IImportEntity.ExternalID)
            }
        };

        return _context.BulkInsertOrUpdateAsync(data.ToList(), config, cancellationToken: token);
    }
    
    
}