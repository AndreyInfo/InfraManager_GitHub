using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.Settings;
using InfraManager.DAL;

namespace InfraManager.BLL.ColumnMapper;

public class OrderedColumnQueryBuilder<TEntity, TTable> : IOrderedColumnQueryBuilder<TEntity, TTable>
    where TEntity : class
{
    private readonly IUserColumnSettingsBLL _columnBLL;
    private readonly ICurrentUser _currentUser;
    private readonly IColumnMapper<TEntity, TTable> _columnMapper;
    
    public OrderedColumnQueryBuilder(IUserColumnSettingsBLL columnBLL,
        ICurrentUser currentUser,
        IColumnMapper<TEntity, TTable> columnMapper)
    {
        _columnBLL = columnBLL;
        _currentUser = currentUser;
        _columnMapper = columnMapper;
    }

    public async Task<IOrderedQueryable<TEntity>> BuildQueryAsync(
        string viewName,
        IQueryable<TEntity> query,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(viewName))
        {
            return query.OrderBy(x => 0);
        }
        var columns = await _columnBLL.GetAsync(_currentUser.UserId, viewName, cancellationToken);
        var orderColumn = columns.GetSortColumn();

        var mappedValues = _columnMapper.MapToRightColumn(orderColumn.PropertyName);

        return BuildQuery(orderColumn, mappedValues, query);
    }
    
    private IOrderedQueryable<TEntity> BuildQuery(Sort orderProperty,
        Expression<Func<TEntity, object>>[] mappedValues,
        IQueryable<TEntity> query)
    {
        if (mappedValues == null)
        {
            return query.OrderBy(orderProperty);
        }

        IOrderedQueryable<TEntity> _orderedQuery;
        if (orderProperty.Ascending)
        {
            _orderedQuery = query.OrderBy(mappedValues[0]);
        }
        else
        {
            _orderedQuery = query.OrderByDescending(mappedValues[0]);
        }
            
        for (int i = 1; i < mappedValues.Length; i++)
        {
            if (orderProperty.Ascending)
            {
                _orderedQuery = _orderedQuery.ThenBy(mappedValues[i]);
            }
            else
            {
                _orderedQuery = _orderedQuery.ThenByDescending(mappedValues[i]);
            }
        }

        return _orderedQuery;
    }
}