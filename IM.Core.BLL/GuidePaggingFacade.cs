using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.ColumnMapper;
using InfraManager.BLL.Settings;
using InfraManager.Core.Extensions;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL;

namespace InfraManager.BLL
{
    public class GuidePaggingFacade<TEntity, TTable> : IGuidePaggingFacade<TEntity, TTable> where TEntity : class
    {
        private IQueryable<TEntity> _query;
        private readonly IPagingQueryCreator _pagging;
        private readonly IOrderedColumnQueryBuilder<TEntity, TTable> _orderedColumnQueryBuilder;

        public GuidePaggingFacade(IPagingQueryCreator paging,
            IOrderedColumnQueryBuilder<TEntity, TTable> orderedColumnQueryBuilder)
        {
            _pagging = paging;
            _orderedColumnQueryBuilder = orderedColumnQueryBuilder;
        }
        
        public async Task<TEntity[]> GetPaggingAsync(BaseFilter filter,
            IQueryable<TEntity> query,
            Expression<Func<TEntity, bool>> searchPredicate = null,
            CancellationToken cancellationToken = default)
        {
            if (query != null)
            {
                _query = query;
            }

            if (searchPredicate != null && !string.IsNullOrEmpty(filter.SearchString))
            {
                _query = _query.Where(searchPredicate);
            }

            var orderedQuery =
                await _orderedColumnQueryBuilder.BuildQueryAsync(filter.ViewName, _query, cancellationToken);

            var paggingQuery = _pagging.Create(orderedQuery);
            return await paggingQuery.PageAsync(filter.StartRecordIndex, filter.CountRecords, cancellationToken);
        }
    }
}
