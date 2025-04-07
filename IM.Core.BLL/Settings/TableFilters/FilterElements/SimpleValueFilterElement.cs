using InfraManager.DAL;
using InfraManager.DAL.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace InfraManager.BLL.Settings.TableFilters.FilterElements
{
    internal class SimpleValueFilterElement : FilterElementBase
    {
        public SimpleValueFilterElement(
            FilterElementDetails model,
            IArrayExpressionBuilder expressionBuilder) 
            : base(model)
        {
            _expressionBuilder = expressionBuilder;
            SelectedValues = model.SelectedValues?.ToArray();
            ClassSearcher = model.ClassSearcher;
            SearcherParams = model.SearcherParams;
            IsFilteredBySearcher = model.IsFilteredBySearcher;
            UseSelf = model.UseSelf;
            Operation = (SimpleValueSearchOperation?)model.Operation;
        }

        public SimpleValueFilterElement(
            FilterElementData elementData, 
            TreeSettings treeSettings,
            IArrayExpressionBuilder expressionBuilder) : base(elementData)
        {
            _expressionBuilder = expressionBuilder;
            SelectedValues = elementData.TreeValues?.ToArray();
            ClassSearcher = elementData.ClassSearcher;
            SearcherParams = elementData.SearcherParams?.ToArray();
            UseSelf = elementData.UseSelf;
            Operation = (SimpleValueSearchOperation?)elementData.SearchOperation;
            Options = treeSettings;
            IsFilteredBySearcher = elementData.IsFilteredBySearcher;
        }

        public TreeValueData[] SelectedValues { get; init; }
        public string ClassSearcher { get; private set; }
        public string[] SearcherParams { get; private set; }
        public bool UseSelf { get; private set; }
        public SimpleValueSearchOperation? Operation { get; private set; }
        public TreeSettings Options { get; private set; }
        public bool? IsFilteredBySearcher { get; private set; }

        protected override void AssignDataAttributes(FilterElementData data)
        {
            data.SearchOperation = (byte?)Operation;
            data.TreeValues = SelectedValues?.ToList();
            data.ClassSearcher = ClassSearcher;
            data.SearcherParams = SearcherParams?.ToList();
            data.UseSelf = UseSelf;
            data.IsFilteredBySearcher = IsFilteredBySearcher;
        }

        private readonly IArrayExpressionBuilder _expressionBuilder;

        protected override Expression<Func<TProperty, bool>> OperationPredicate<TProperty>(
            IEnumerable<IBuildPredicateParameter> parameterBuilders)
        {
            return _expressionBuilder.Build<TProperty>(
                SelectedValues.Select(x => x.ID),
                parameterBuilders,
                Operation == SimpleValueSearchOperation.NotEqual);
        }
    }
}
