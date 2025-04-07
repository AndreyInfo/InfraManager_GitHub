using System.Linq.Expressions;

namespace InfraManager.BLL.ExpressionBuilder
{
    /// <summary>
    /// Этот интерфейс описывает члены построителя выражения вызова функции
    /// </summary>
    public interface IFunctionExpressionBuilder
    {
        /// <summary>
        /// Возвращает имя функции
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Строит выражение вызова функции
        /// </summary>
        /// <param name="parameter">Параметр выражения</param>
        /// <param name="arguments">Выражения, вычисляющие аргументы функции</param>
        /// <returns>Выражение вызова функции</returns>
        Expression Build(ParameterExpression parameter, params Expression[] arguments);
    }
}
