using InfraManager.DAL.Settings;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace InfraManager.BLL.Settings.TableFilters.FilterElements
{
    public class DefaultFilterElement : FilterElementBase
    {
        public DefaultFilterElement(FilterElementData elementData) : base(elementData)
        {
        }

        public DefaultFilterElement(FilterElementDetails model) : base(model)
        {
        }

        protected override void AssignDataAttributes(FilterElementData data)
        {
        }

        protected override Expression<Func<TProperty, bool>> OperationPredicate<TProperty>(
            IEnumerable<IBuildPredicateParameter> parameterBuilders)
        {
            throw new NotImplementedException();
        }
    }
}
