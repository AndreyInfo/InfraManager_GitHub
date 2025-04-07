using System;
using System.Linq.Expressions;

namespace InfraManager.BLL.ColumnMapper;


public class ColumnMapper<TEntity, TTable> : IColumnMapper<TEntity, TTable>
{
    private readonly ColumnMapperSettings<TEntity,TTable> _settingsBase;
    public ColumnMapper(ColumnMapperSettings<TEntity,TTable> columnMapperSettingsBase)
    {
        _settingsBase = columnMapperSettingsBase;
    }
    
    public Expression<Func<TEntity, object>>[] MapToRightColumn(string tablePropertyName)
    {
       return _settingsBase.FindByKey(tablePropertyName);
    }
}