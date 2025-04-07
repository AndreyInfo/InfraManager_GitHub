using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace InfraManager.BLL.Settings.TableFilters.FilterElements
{
    public interface IArrayExpressionBuilder
    {
        Expression<Func<TProperty, bool>> Build<TProperty>(
            IEnumerable<string> selectedValues,
            IEnumerable<IBuildPredicateParameter> parameterBuilders,
            bool excluding);
    }
}
