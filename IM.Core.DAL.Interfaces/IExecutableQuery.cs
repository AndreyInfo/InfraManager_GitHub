using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL;

public interface IExecutableQuery<T> : IQueryable<T>
{
    Task<T[]> ExecuteAsync(CancellationToken cancellationToken);
    IExecutableQuery<T> Where(Expression<Func<T, bool>> predicate);
}
