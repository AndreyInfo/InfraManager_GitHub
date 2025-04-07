using InfraManager.BLL.Settings.TableFilters.FilterElements.ExpressionBuilders;
using InfraManager.DAL.Settings;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace InfraManager.BLL.Settings.TableFilters.FilterElements
{
    internal class RangeSliderFilterElement : FilterElementBase
    {
        #region .ctor

        private const string FloatFormat = "0.00";

        public RangeSliderFilterElement(
            FilterElementData elementData,
            IParser parser) 
            : base(elementData)
        {
            Left = elementData.Left;
            Right = elementData.Right;
            Operation = (RangeSearchOperation?)elementData.SearchOperation;
            _parser = parser;
        }

        public RangeSliderFilterElement(
            FilterElementDetails model,
            IParser parser) 
            : base(model)
        {
            Left = model.Left;
            Right = model.Right;
            Operation = (RangeSearchOperation?)model.Operation;
            _parser = parser;
        }

        #endregion

        #region Properties

        public RangeSearchOperation? Operation { get; }

        public bool IsFloat { get; }

        public string Left { get; }

        public string Right { get; }

        public string Min => IsFloat 
            ? float.MinValue.ToString(FloatFormat) 
            : int.MinValue.ToString();

        public string Max => IsFloat 
            ? float.MaxValue.ToString(FloatFormat) 
            : int.MaxValue.ToString();

        #endregion

        #region Assign data attributes

        protected override void AssignDataAttributes(FilterElementData data)
        {
            data.SearchOperation = (byte?)Operation;
            data.Left = Left;
            data.Right = Right;
        }

        #endregion

        #region Build predicate

        private static Dictionary<RangeSearchOperation, IRangeExpressionBuilder> _builders =
            new Dictionary<RangeSearchOperation, IRangeExpressionBuilder>
            {
                { RangeSearchOperation.Between, new BetweenExpressionBuilder() },
                { RangeSearchOperation.Equal, new EqualExpressionBuilder() },
                { RangeSearchOperation.Except, new ExceptExpressionBuilder() },
                { RangeSearchOperation.Less, new LessExpressionBuilder() },
                { RangeSearchOperation.More, new GreaterExpressionBuilder() },
                { RangeSearchOperation.NotEqual, new NotEqualExpressionBuilder() }
            };

        private readonly IParser _parser;

        protected override Expression<Func<TProperty, bool>> OperationPredicate<TProperty>(
            IEnumerable<IBuildPredicateParameter> parameterBuilders)
        {
            return _builders[(RangeSearchOperation)Operation].Build(
                _parser.Parse<TProperty>(Left),
                _parser.Parse<TProperty>(Right));
        }

        #endregion
    }
}
