using InfraManager.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace InfraManager.BLL.Settings.TableFilters
{
    internal class FilterBuildersAggregate<TBase, TTarget> : IAggregatePredicateBuilders<TBase, TTarget>
    {
        #region Predicate builders dictionary

        private readonly Dictionary<string, IBuildPredicate<TBase>> _builders =
            new Dictionary<string, IBuildPredicate<TBase>>();

        public FilterBuildersAggregate<TBase, TTarget> AddPredicateBuilder(
            string memberName, 
            IBuildPredicate<TBase> predicateBuilder)
        {
            _builders.Add(memberName, predicateBuilder);

            return this;
        }

        public FilterBuildersAggregate<TBase, TTarget> AddPredicateBuilder<TProperty>(
            string memberName,
            Expression<Func<TBase, TProperty>> property)
        {
            return AddPredicateBuilder(
                memberName,
                new DefaultPredicateBuilder<TBase, TProperty>(property, _parameters));
        }

        public FilterBuildersAggregate<TBase, TTarget> AddPredicateBuilder<TProperty>(
            Expression<Func<TBase, TProperty>> property)
        {
            if (property.TryGetPropertyName(out var memberName))
            {
                return AddPredicateBuilder(
                    memberName,
                    new DefaultPredicateBuilder<TBase, TProperty>(property, _parameters));
            }

            throw new NotSupportedException($"Cannot identify member name from {property}");
        }

        public FilterBuildersAggregate<TBase, TTarget> AddPredicateBuilder<TSourceProperty, TTargetProperty>(
            Expression<Func<TTarget, TTargetProperty>> target,
            Expression<Func<TBase, TSourceProperty>> property)
        {
            if (target.TryGetPropertyName(out var targetPropertyName))
            {
                return AddPredicateBuilder(targetPropertyName, property);
            }

            throw new NotSupportedException($"Cannot identify target property.");
        }

        #endregion

        #region IAggregateFilterBuilders<TBase> implementation

        public IBuildPredicate<TBase> this[string memberName] => _builders[memberName];

        public Expression<Func<TBase, bool>> Build(FilterElementBase filter)
        {
            return _builders[filter.PropertyName].Build(filter);
        }

        public bool Contains(string memberName)
        {
            return _builders.ContainsKey(memberName);
        }

        private readonly List<IBuildPredicateParameter> _parameters =
            new List<IBuildPredicateParameter>();

        public IAggregatePredicateBuilders<TBase, TTarget> AddParameter(IBuildPredicateParameter parameter)
        {
            _parameters.Add(parameter);

            return this;
        }

        #endregion
    }
}
