using System.Linq.Expressions;

namespace InfraManager.BLL.ExpressionBuilder
{
    /// <summary>
    /// Этот интерфейс описывает члены построителя выражения бинарного оператора
    /// </summary>
    public interface IBinaryOperatorExpressionBuilder
    {
        /// <summary>
        /// Возвращает символьное представление оператора
        /// </summary>
        char Operator { get; }

        /// <summary>
        /// Возвращает значение приоритета оператора в выражении
        /// </summary>
        byte Priority { get; }
        
        /// <summary>
        /// Строит выражение для соответствующего оператора
        /// </summary>
        /// <param name="left">Выражение слева от оператора</param>
        /// <param name="right">Выражение справа от оператора</param>
        /// <returns>Итоговое выражение left x right</returns>
        Expression Build(Expression left, Expression right);
    }
}
