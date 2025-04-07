using System.Linq.Expressions;

namespace InfraManager.BLL.ExpressionBuilder
{
    /// <summary>
    /// Этот интерфейс описывает члены строителя выражений для доступна к переменным
    /// </summary>
    /// <typeparam name="TArg"></typeparam>
    public interface IVariableExpressionBuilder<TArg>
    {
        /// <summary>
        /// Возвращает имя переменной
        /// </summary>
        string Variable { get; }

        /// <summary>
        /// Строит выражение доступа к переменной
        /// </summary>
        /// <param name="parameter">Ссылка на параметр выражения</param>
        /// <returns>Выражение доступа к значению переменной</returns>
        Expression Build(ParameterExpression parameter);
    }
}
