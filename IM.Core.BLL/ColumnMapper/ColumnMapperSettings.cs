using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using InfraManager.Linq;

namespace InfraManager.BLL.ColumnMapper;

public class ColumnMapperSettings<TEntity, TTable> : IColumnMapperSettingsBase<TEntity, TTable>
{    
    private readonly Dictionary<string, List<Expression<Func<TEntity, object>>>> _specialColumns = new();
    private readonly IColumnMapperSetting<TEntity, TTable> _settings;
    
    public ColumnMapperSettings(IColumnMapperSetting<TEntity, TTable> settings)
    {
        _settings = settings;
    }

    public IThenColumnMapper<TEntity, TTable> ShouldBe(Expression<Func<TTable, object>> tableProperty,
        Expression<Func<TEntity, object>> entityProperty)
    {
        string propertyTableName = tableProperty.GetPropertyName();
        
        if (!_specialColumns.ContainsKey(propertyTableName))
        {
            _specialColumns.Add(propertyTableName, new List<Expression<Func<TEntity, object>>> { entityProperty });
        }
        else
        {
            var setting = _specialColumns[propertyTableName];

            if (setting.Any(x => x.Equals(entityProperty)))
            {
                throw new NotSupportedException("Key and value duplicate column settings");
            }
            
            setting.Add(entityProperty);
        }

        return new ThenColumnMapper<TEntity, TTable>(this, tableProperty);
    }


    public class ThenColumnMapper<TEntity, TTable> : IThenColumnMapper<TEntity, TTable>
    {
        private readonly ColumnMapperSettings<TEntity, TTable> _columnSettings;
        private readonly Expression<Func<TTable, object>> _tableProperty;

        public ThenColumnMapper(ColumnMapperSettings<TEntity, TTable> columnSettings,
            Expression<Func<TTable, object>> tableProperty)
        {
            _tableProperty = tableProperty;
            _columnSettings = columnSettings;
        }
        
        
        public IThenColumnMapper<TEntity, TTable> Then(Expression<Func<TEntity, object>> entityProperty)
        {
            _columnSettings.ShouldBe(_tableProperty, entityProperty);
            return this;
        }
    }
    
    
    public Expression<Func<TEntity, object>>[] FindByKey(string tablePropertyName)
    {
        _settings.Configure(this);
        if (_specialColumns.TryGetValue(tablePropertyName, out List<Expression<Func<TEntity, object>>> result))
        {
            return result.ToArray();
        }

        return null;
    }
}