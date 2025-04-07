using InfraManager.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace InfraManager.BLL.Settings.TableFilters.FilterElements.ExpressionBuilders
{
    public class ArrayExpressionBuilder : 
        IArrayExpressionBuilder,
        ISelfRegisteredService<IArrayExpressionBuilder>
    {
        private readonly IParser _parser;

        public ArrayExpressionBuilder(IParser parser)
        {
            _parser = parser;
        }

        public Expression<Func<TProperty, bool>> Build<TProperty>(
            IEnumerable<string> selectedValues, 
            IEnumerable<IBuildPredicateParameter> parameterBuilders, 
            bool excluding)
        {
            var parameter = Expression.Parameter(typeof(TProperty));
            Expression body = null;

            TProperty[] constants = selectedValues
                .Where(s => parameterBuilders.FirstOrDefault(p => p.Matches(s)) == null)
                .Select(s => _parser.Parse<TProperty>(s))
                .ToArray();

            if (constants.Any())
            {
                Expression<Func<TProperty, bool>> baseExpression = id => constants.Contains(id);
                body = new ExpressionReplacer(baseExpression.Parameters[0], parameter)
                    .Visit(baseExpression.Body);
            }

            foreach (var expression in selectedValues
                .Select(
                    s => parameterBuilders
                        .FirstOrDefault(p => p.Matches(s))
                        ?.GetExpression<TProperty>(s, parameter))
                .Where(x => x != null))
            {
                body = body == null ? expression : Expression.Or(body, expression);
            }

            body = excluding ? Expression.Not(body) : body;

            return Expression.Lambda<Func<TProperty, bool>>(body, parameter);
        }
    }
}
