using System;
using System.Linq.Expressions;

namespace InfraManager.BLL.ColumnMapper;

public interface IColumnMapperSettingsBase<TEntity, TTable>
{
    public IThenColumnMapper<TEntity, TTable> ShouldBe(Expression<Func<TTable, object>> tableProperty,
        Expression<Func<TEntity, object>> entityProperty);
}