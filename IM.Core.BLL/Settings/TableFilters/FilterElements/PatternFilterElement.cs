using InfraManager.DAL.Settings;
using InfraManager.Expressions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace InfraManager.BLL.Settings.TableFilters.FilterElements
{
    internal class PatternFilterElement : FilterElementBase
    {
        private const int maxLength = 100;

        public string SearchValue { get; private set; }
        public PatternSearchOperation? Operation { get; private set; }

        public PatternFilterElement(FilterElementData elementData) : base(elementData)
        {
            SearchValue = TruncateSearchValue(elementData.SearchValue);
            Operation = (PatternSearchOperation?)elementData.SearchOperation;
            InitPredicate();
        }

        public PatternFilterElement(FilterElementDetails model) : base(model)
        {
            SearchValue = model.SearchValue;
            Operation = (PatternSearchOperation?)model.Operation;
            InitPredicate();
        }

        private static string TruncateSearchValue(string searchValue)
        {
            return string.IsNullOrEmpty(searchValue)
                ? null
                : searchValue.Length > maxLength ? searchValue.Substring(0, maxLength) : searchValue;
        }

        protected override void AssignDataAttributes(FilterElementData data)
        {
            data.SearchOperation = (byte?)Operation;
            data.SearchValue = TruncateSearchValue(SearchValue);
        }

        private Expression<Func<string, bool>> _predicate;

        private void InitPredicate()
        {
            if (Operation == PatternSearchOperation.Contains)
            {
                _predicate = s => s.ToLower().Contains(SearchValue.ToLower());
            }
            else
            {
                _predicate = s => s.ToLower() == SearchValue.ToLower();
            }
        }

        protected override Expression<Func<TProperty, bool>> OperationPredicate<TProperty>(
            IEnumerable<IBuildPredicateParameter> parameterBuilders)
        {
            if (typeof(TProperty) == typeof(string))
            {
                return Expression.Lambda<Func<TProperty, bool>>(_predicate.Body, _predicate.Parameters[0]);
            }

            return ExpressionReplacer
                .ReplaceParameterWithExpression<TProperty, string, bool>(_predicate, x => x.ToString());
        }
    }
}
