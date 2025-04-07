using System.Linq.Expressions;

namespace InfraManager.BLL.Settings.TableFilters.FilterElements.ExpressionBuilders
{
    public class LessExpressionBuilder : RangeExpressionBuilderBase
    {
        protected override Expression BuildBody(
            ParameterExpression parameter, 
            Expression min, 
            Expression max)
        {
            return Expression.LessThan(parameter, min);
        }
    }
}
