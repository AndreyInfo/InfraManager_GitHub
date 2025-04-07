using System.Linq.Expressions;

namespace InfraManager.BLL.ExpressionBuilder.Operators
{
    /// <summary>
    /// Этот класс реализует построитель выражения целочисленного деления
    /// </summary>
    public class DivideExpressionBuilder<TResult> : IBinaryOperatorExpressionBuilder where TResult : struct
    {
        public char Operator => '/';

        public byte Priority => 2;

        public Expression Build(Expression left, Expression right)
        {
            if (right.IsConstantZero<TResult>())
            {
                throw new ExpressionValidationException(
                    ExpressionValidationException.ZeroDivision);
            }

            return Expression.Divide(left, right);
        }
    }
}
