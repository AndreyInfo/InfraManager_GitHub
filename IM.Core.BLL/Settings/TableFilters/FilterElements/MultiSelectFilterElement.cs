using InfraManager.DAL;
using InfraManager.DAL.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace InfraManager.BLL.Settings.TableFilters.FilterElements
{
    internal class MultiSelectFilterElement : FilterElementBase
    {       
        public ValueData[] SelectedValues { get; }

        public MultiSelectSearchOperation? Operation { get; }

        public MultiSelectFilterElement(
            FilterElementData elementData,
            IArrayExpressionBuilder expressionBuilder) 
            : base(elementData)
        {
            _expressionBuilder = expressionBuilder;
            SelectedValues = elementData.SelectedValues?.ToArray();
            Operation = (MultiSelectSearchOperation?)elementData.SearchOperation;
        }

        public MultiSelectFilterElement(
            FilterElementDetails model,
            IArrayExpressionBuilder expressionBuilder) 
            : base(model)
        {
            _expressionBuilder = expressionBuilder;
            SelectedValues = model.SelectedValues?.Cast<ValueData>().ToArray();
            Operation = (MultiSelectSearchOperation?)model.Operation;
        }

        protected override void AssignDataAttributes(FilterElementData data)
        {
            data.SearchOperation = (byte?)Operation;
            data.SelectedValues = SelectedValues?.ToList();
        }

        private readonly IArrayExpressionBuilder _expressionBuilder;

        protected override Expression<Func<TProperty, bool>> OperationPredicate<TProperty>(
            IEnumerable<IBuildPredicateParameter> parameterBuilders)
        {
            return _expressionBuilder.Build<TProperty>(
                SelectedValues.Select(x => x.ID),
                parameterBuilders,
                Operation == MultiSelectSearchOperation.NotEqual);
        }
    }
}
