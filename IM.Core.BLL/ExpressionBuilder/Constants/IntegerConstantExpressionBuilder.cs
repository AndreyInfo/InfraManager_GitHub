using System.Linq.Expressions;

namespace InfraManager.BLL.ExpressionBuilder.Constants
{
    /// <summary>
    /// Этот класс реализует построитель выражения целочисленной константы
    /// </summary>
    public class IntegerConstantExpressionBuilder : IConstantExpressionBuilder<int>
    {
        public bool TryBuild(string text, out Expression expression)
        {
            expression = null;
            if (int.TryParse(text.Trim(), out int value))
            {
                expression = Expression.Constant(value);
                return true;
            }

            return false;
        }
    }
}
