using System.Linq.Expressions;

namespace InfraManager.BLL.Settings.TableFilters.FilterElements.ExpressionBuilders
{
    public class GreaterExpressionBuilder : RangeExpressionBuilderBase
    {
        protected override Expression BuildBody(
            ParameterExpression parameter, 
            Expression min, 
            Expression max)
        {
            return Expression.GreaterThan(parameter, min);
        }
    }
}
