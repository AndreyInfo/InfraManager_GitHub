using System.Linq.Expressions;

namespace InfraManager.BLL.ExpressionBuilder.Operators
{
    /// <summary>
    /// Этот класс реализует построитель выражения умножения
    /// </summary>
    public class MultiplyExpressionBuilder : IBinaryOperatorExpressionBuilder
    {
        public char Operator => '*';

        public byte Priority => 2;

        public Expression Build(Expression left, Expression right)
        {
            return Expression.Multiply(left, right);
        }
    }
}
