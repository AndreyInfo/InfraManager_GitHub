using System;
using System.Linq.Expressions;

namespace InfraManager.BLL.Settings.TableFilters.FilterElements
{
    public interface IRangeExpressionBuilder
    {
        Expression<Func<TProperty, bool>> Build<TProperty>(TProperty min, TProperty max);
    }
}
