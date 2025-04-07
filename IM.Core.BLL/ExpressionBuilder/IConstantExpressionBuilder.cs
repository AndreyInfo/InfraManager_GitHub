using System.Linq.Expressions;

namespace InfraManager.BLL.ExpressionBuilder
{
    /// <summary>
    /// Этот интерфейс описывает члены построите выражений-констант
    /// </summary>
    /// <typeparam name="T">Тип константы</typeparam>
    public interface IConstantExpressionBuilder<T> where T : struct
    {
        /// <summary>
        /// Пытается преобразовать строку к выражению. представляющему константу определенного типа
        /// </summary>
        /// <param name="text">Значение</param>
        /// <param name="expression">Выражение</param>
        /// <returns>Истина если строку удается преобразовать к константе определенного типа, ложь - если не удается</returns>
        bool TryBuild(string text, out Expression expression);
    }
}
