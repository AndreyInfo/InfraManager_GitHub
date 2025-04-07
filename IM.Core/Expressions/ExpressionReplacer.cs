using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace InfraManager.Expressions
{
    public class ExpressionReplacer : ExpressionVisitor
    {
        private readonly Expression _target;
        private readonly Expression _replacement;

        public ExpressionReplacer(Expression target, Expression replacement)
        {
            _target = target;
            _replacement = replacement;
        }

        public Expression Replace(Expression source)
        {
            return Visit(source);
        }

        [return: NotNullIfNotNull("node")]
        public override Expression Visit(Expression node)
        {
            return ReferenceEquals(_target, node) ? _replacement : base.Visit(node);
        }

        /// <summary>
        /// Replaces expression parameter x in f(x) with another expression g(y).
        /// Example: g(y) = y + 2; f(x) = x * x; f(g) => f(y) = (y + 2) * (y + 2)
        /// </summary>
        /// <typeparam name="TStart">g(x) parameter type</typeparam>
        /// <typeparam name="T">g(x) result type and f(x) parameter type (they should match)</typeparam>
        /// <typeparam name="TFinish">f(x) result type</typeparam>
        /// <param name="expression">f(x) expression</param>
        /// <param name="parameter">g(y) expression</param>
        /// <returns>f(g)</returns>
        public static Expression<Func<TStart, TFinish>> ReplaceParameterWithExpression<TStart, T, TFinish>(
            Expression<Func<T, TFinish>> expression,
            Expression<Func<TStart, T>> parameter)
        {
            var replacer = new ExpressionReplacer(expression.Parameters[0], parameter.Body);
            var body = replacer.Replace(expression.Body);

            return Expression.Lambda<Func<TStart, TFinish>>(body, parameter.Parameters[0]);
        }
    }
}
