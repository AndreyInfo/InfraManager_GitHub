using System.Linq.Expressions;

namespace InfraManager.BLL.Settings.TableFilters.FilterElements.ExpressionBuilders
{
    public class NotEqualExpressionBuilder : RangeExpressionBuilderBase
    {
        protected override Expression BuildBody(
            ParameterExpression parameter, 
            Expression min, 
            Expression max)
        {
            return Expression.Not(Expression.Equal(parameter, min));
        }
    }
}
