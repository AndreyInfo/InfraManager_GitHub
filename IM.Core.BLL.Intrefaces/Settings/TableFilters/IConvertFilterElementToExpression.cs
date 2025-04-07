using System;
using System.Linq.Expressions;

namespace InfraManager.BLL.Settings.TableFilters
{
    public interface IConvertFilterElementToExpression<TProperty>
    {
        Expression<Func<TProperty, bool>> Convert(FilterElementBase filter);
    }
}
