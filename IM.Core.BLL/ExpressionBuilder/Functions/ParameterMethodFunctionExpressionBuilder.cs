using System.Linq.Expressions;

namespace InfraManager.BLL.ExpressionBuilder.Functions
{
    /// <summary>
    /// Этот класс реализует строителя выражения вызова метода аргумента выражения
    /// <typeparamref name="T">Тип параметра выражения</typeparamref>
    /// </summary>
    public class ParameterMethodFunctionExpressionBuilder<T> : IFunctionExpressionBuilder
    {
        private readonly string _method;

        /// <summary>
        /// Создает экземпляр типа ParameterMethodFunctionExpressionBuilder
        /// </summary>
        /// <param name="name">Имя функции (в выражении)</param>
        /// <param name="method">Имя метода параметра выражения (который соотв. этой функции)</param>
        public ParameterMethodFunctionExpressionBuilder(string name, string method = null)
        {
            Name = name;
            _method = method ?? name;
        }

        public string Name { get; private set; }

        public Expression Build(ParameterExpression parameter, params Expression[] arguments)
        {
            var methodInfo = typeof(T).GetMethod(_method);

            if (arguments.Length != methodInfo.GetParameters().Length)
            {
                throw new ExpressionValidationException(
                    ExpressionValidationException.IncorrectParametersQuantity,
                    Name,
                    methodInfo.GetParameters().Length,
                    arguments.Length);
            }

            return Expression.Call(parameter, methodInfo, arguments);
        }
    }
}
