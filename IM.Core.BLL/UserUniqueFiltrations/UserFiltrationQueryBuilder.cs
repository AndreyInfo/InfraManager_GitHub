using System;
using System.Linq.Expressions;
using InfraManager.BLL.ColumnMapper;
using InfraManager.BLL.Events;
using InfraManager.DAL;
using InfraManager.Linq;
using ExpressionExtensions = InfraManager.Linq.ExpressionExtensions;

namespace InfraManager.BLL.UserUniqueFiltrations;

public class UserFiltrationQueryBuilder<Entity, Table> : IUserFiltrationQueryBuilder<Entity,Table> where Entity : class
{
    private IExecutableQuery<Entity> _query;
    private readonly IColumnMapper<Entity, Table> _columnMapper;

    public UserFiltrationQueryBuilder(IReadonlyRepository<Entity> query,
        IColumnMapper<Entity, Table> columnMapper)
    {
        _columnMapper = columnMapper;
        _query = query.Query();
    }

    public IExecutableQuery<Entity> Build(PersonalUserFiltrationItem[] columnsParams,
        IExecutableQuery<Entity> query = null)
    {
        if (query != null)
        {
            _query = query;
        }
        
        foreach (var el in columnsParams)
        {
            if (el.SearchValues == null)
            {
                continue;
            }

            Expression<Func<Entity, object>> expression;
            
            var columns = _columnMapper.MapToRightColumn(el.SearchColumnName);

            if (columns != null)
            {
                expression = columns[0];
            }
            else
            {
                expression = ExpressionExtensions.WithNestedProperty<Entity>(el.SearchColumnName);
            }

            foreach (var searchValue in el.SearchValues)
            {
                var comparerExpression = expression.BuildPredicate(el.FiltrationAction, searchValue);

                _query = _query.Where(comparerExpression);
            }
        }

        return _query;
    }
}