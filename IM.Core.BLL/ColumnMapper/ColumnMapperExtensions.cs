using System;
using System.Collections.Generic;
using InfraManager.Linq;

namespace InfraManager.BLL.ColumnMapper;

public static class ColumnMapperExtensions
{
    [Obsolete("Use OrderedColumnQueryBuilder instead of this")]
    public static string MapFirst<TEntity, TTable>(this IColumnMapper<TEntity, TTable> columnMapper, string tableName)
    {
        var mapped = columnMapper.MapToRightColumn(tableName);
        
        if (mapped == null)
        {
            return tableName;
        }
        
        var values = ExpressionExtensions.PropertyPathRecurse(columnMapper.MapToRightColumn(tableName)[0]);
        return string.Join(".", values);
    }

    [Obsolete("Use OrderedColumnQueryBuilder instead of this")]
    public static string[] MapToStringArray<TEntity, TTable>(this IColumnMapper<TEntity, TTable> columnMapper,
        string tableName)
    {
        var mappedValues = columnMapper.MapToRightColumn(tableName);
        
        if (mappedValues == null)
        {
            return new[] { tableName };
        }
        
        var result = new List<string>();

        foreach (var el in mappedValues)
        {
            result.Add(string.Join(".",
                ExpressionExtensions.PropertyPathRecurse(el)));
        }

        return result.ToArray();
    }
}