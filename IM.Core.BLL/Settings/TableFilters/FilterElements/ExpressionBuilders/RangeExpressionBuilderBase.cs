using System;
using System.Linq.Expressions;

namespace InfraManager.BLL.Settings.TableFilters.FilterElements.ExpressionBuilders
{
    public abstract class RangeExpressionBuilderBase : IRangeExpressionBuilder
    {
        public Expression<Func<TProperty, bool>> Build<TProperty>(TProperty min, TProperty max)
        {
            var parameter = Expression.Parameter(typeof(TProperty));

            return Expression.Lambda<Func<TProperty, bool>>(
                BuildBody(parameter, Expression.Convert(Expression.Constant(min),typeof(TProperty)), Expression.Convert(Expression.Constant(max), typeof(TProperty))), 
                parameter);
        }

        protected abstract Expression BuildBody(
            ParameterExpression parameter,
            Expression min,
            Expression max);
    }
}
