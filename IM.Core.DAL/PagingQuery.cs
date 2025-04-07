using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL
{
    internal class PagingQuery<T> : IPagingQuery<T>
    {
        private readonly IOrderedQueryable<T> _query;

        public PagingQuery(IOrderedQueryable<T> query)
        {
            _query = query;
        }

        public Task<T[]> PageAsync(int skip, int take, CancellationToken cancellationToken = default)
        {
            var paging = _query.Skip(skip);

            if (take > 0)
            {
                paging = paging.Take(take);
            }

            return paging.ToArrayAsync(cancellationToken);
        }
    }
}
