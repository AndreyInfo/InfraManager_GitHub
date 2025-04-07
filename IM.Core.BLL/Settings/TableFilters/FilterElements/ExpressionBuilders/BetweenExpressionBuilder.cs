using System.Linq.Expressions;

namespace InfraManager.BLL.Settings.TableFilters.FilterElements.ExpressionBuilders
{
    public class BetweenExpressionBuilder : RangeExpressionBuilderBase
    {
        protected override Expression BuildBody(
            ParameterExpression parameter, 
            Expression min, 
            Expression max)
        {
            return Expression.And(
                Expression.GreaterThanOrEqual(parameter, min),
                Expression.LessThanOrEqual(parameter, max));
        }
    }
}
