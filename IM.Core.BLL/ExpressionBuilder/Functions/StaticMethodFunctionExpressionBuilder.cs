using System.Linq.Expressions;
using System.Reflection;

namespace InfraManager.BLL.ExpressionBuilder.Functions
{
    /// <summary>
    /// Этот класс реализует построитель выражения вызова функции, которая реализована статическим методом
    /// </summary>
    public class StaticMethodFunctionExpressionBuilder : IFunctionExpressionBuilder
    {
        private readonly MethodInfo _method;

        public StaticMethodFunctionExpressionBuilder(MethodInfo method)
        {
            _method = method;
        }

        public string Name => _method.Name;

        public Expression Build(ParameterExpression parameter, params Expression[] arguments)
        {
            if (arguments.Length != _method.GetParameters().Length)
            {
                throw new ExpressionValidationException(
                    ExpressionValidationException.IncorrectParametersQuantity,
                    Name,
                    _method.GetParameters().Length,
                    arguments.Length);
            }

            return Expression.Call(_method, arguments);
        }
    }
}
