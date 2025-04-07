using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL;

namespace InfraManager.BLL;

public interface IGuidePaggingFacade<TEntity, TTable>
{
    public Task<TEntity[]> GetPaggingAsync(BaseFilter filter,
        IQueryable<TEntity> query,
        Expression<Func<TEntity, bool>> searchPredicate = null,
        CancellationToken cancellationToken = default);
}