using System;
using System.Linq.Expressions;

namespace InfraManager.BLL.Settings.TableFilters
{
    internal class FilterElementConvertersAggregate<TProperty> 
        : IConvertFilterElementToExpression<TProperty>
    {
        public Expression<Func<TProperty, bool>> Convert(FilterElementBase filter)
        {
            throw new NotImplementedException();
        }
    }
}
