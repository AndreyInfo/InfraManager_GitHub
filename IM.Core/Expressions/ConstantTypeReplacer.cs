using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace InfraManager.Expressions
{
    public class ConstantTypeReplacer<TConstant, TTarget> : ExpressionVisitor
    {
        [return: NotNullIfNotNull("node")]
        public override Expression Visit(Expression node)
        {
            return node is ConstantExpression constant 
                    && constant.Type == typeof(TConstant)
                ? Expression.Constant(constant.Value, typeof(TTarget))
                : base.Visit(node);
        }
    }
}
