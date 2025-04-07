using System.Linq.Expressions;

namespace InfraManager.BLL.ExpressionBuilder.Operators
{
    /// <summary>
    /// Этот класс реализует построитель выражения вычитания
    /// </summary>
    public class MinusExpressionBuilder : IBinaryOperatorExpressionBuilder
    {
        public char Operator => '-';

        public byte Priority => 1;

        public Expression Build(Expression left, Expression right)
        {
            return Expression.Subtract(left, right);
        }
    }
}
