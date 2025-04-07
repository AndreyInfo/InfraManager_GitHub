using System;

namespace InfraManager.BLL.ExpressionBuilder.Functions
{
    public static class MinExpressionBuilder
    {
        /// <summary>
        /// Создает построитель выражения вызова функции Min
        /// </summary>
        /// <typeparam name="TValue">Тип аргументов и возвращаемого значения</typeparam>
        /// <returns>Построитель выражения вызова функции</returns>
        public static IFunctionExpressionBuilder Create<TValue>()
        {
            return new StaticMethodFunctionExpressionBuilder(
                typeof(Math).GetMethod(nameof(Math.Min), new[] { typeof(TValue), typeof(TValue) }));
        }
    }
}
