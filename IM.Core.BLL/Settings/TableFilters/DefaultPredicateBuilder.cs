using InfraManager.Expressions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace InfraManager.BLL.Settings.TableFilters
{
    public class DefaultPredicateBuilder<TBase, TProperty> : IBuildPredicate<TBase>
    {
        private readonly Expression<Func<TBase, TProperty>> _property;
        private readonly IEnumerable<IBuildPredicateParameter> _parameterBuilders;

        public DefaultPredicateBuilder(
            Expression<Func<TBase, TProperty>> property,
            IEnumerable<IBuildPredicateParameter> parameterBuilders)
        {
            _property = property;
            _parameterBuilders = parameterBuilders;
        }

        public Expression<Func<TBase, bool>> Build(FilterElementBase filter)
        {
            return ExpressionReplacer.ReplaceParameterWithExpression(
                filter.BuildPredicate<TProperty>(_parameterBuilders),
                _property);
        }
    }
}
