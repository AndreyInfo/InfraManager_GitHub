using System.Linq.Expressions;

namespace InfraManager.BLL.ExpressionBuilder.Variables
{
    /// <summary>
    /// Этот класс реализует строителя выражения переменной выражения, которая представляет одно из свойств параметра выражения
    /// </summary>
    /// <typeparam name="T">Тип параметра выражения</typeparam>
    public class PropertyVariableExpressionBuilder<T> : IVariableExpressionBuilder<T>
    {
        private readonly string _propertyName;

        public PropertyVariableExpressionBuilder(string variable, string propertyName = null)
        {
            Variable = variable;
            _propertyName = propertyName ?? variable;
        }

        public string Variable { get; private set; }

        public Expression Build(ParameterExpression parameter)
        {
            return Expression.PropertyOrField(parameter, _propertyName);
        }
    }
}
