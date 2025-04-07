using System.Linq.Expressions;

namespace InfraManager.BLL.ExpressionBuilder
{
    public static class ExpressionExtensions
    {
        public static bool IsConstantZero<T>(this Expression expression) where T : struct
        {
            return expression is ConstantExpression constEx
                && constEx.Value.Equals(default(T));
        }
    }
}
