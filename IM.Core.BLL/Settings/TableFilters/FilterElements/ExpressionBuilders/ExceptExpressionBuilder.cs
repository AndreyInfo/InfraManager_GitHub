using System.Linq.Expressions;

namespace InfraManager.BLL.Settings.TableFilters.FilterElements.ExpressionBuilders
{
    public class ExceptExpressionBuilder : RangeExpressionBuilderBase
    {
        protected override Expression BuildBody(
            ParameterExpression parameter, 
            Expression min, 
            Expression max)
        {
            return Expression.Or(
                Expression.LessThan(parameter, min),
                Expression.GreaterThan(parameter, max));
        }
    }
}
