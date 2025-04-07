using System;
using System.Linq;
using System.Linq.Expressions;

namespace InfraManager.Expressions
{
    /// <summary>
    /// Этот класс преобразует выражения вида Func<T, T1> в Func<T, T2>, где T1 : T2
    /// </summary>
    /// <typeparam name="TNew">Новый тип результата выражения</typeparam>
    public class ExpressionResultConverter<TNew>
    {
        public Expression<Func<T, TNew>> Convert<T, TOriginal>(Expression<Func<T, TOriginal>> expression)
        {
            return Expression.Lambda<Func<T, TNew>>(
                expression.Body,
                expression.Parameters.First());
        }
    }
}   
