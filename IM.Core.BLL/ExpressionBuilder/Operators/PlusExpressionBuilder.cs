using System.Linq.Expressions;

namespace InfraManager.BLL.ExpressionBuilder.Operators
{
    /// <summary>
    /// Этот класс реализует построитель выражения сложения
    /// </summary>
    public class PlusExpressionBuilder : IBinaryOperatorExpressionBuilder
    {
        public char Operator => '+';

        public byte Priority => 1;

        public Expression Build(Expression left, Expression right)
        {
            return Expression.Add(left, right);
        }
    }
}
