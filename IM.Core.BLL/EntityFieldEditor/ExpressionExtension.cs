using System;
using System.Linq.Expressions;

namespace InfraManager.BLL.EntityFieldEditor
{
    public static class ExpressionExtension
    {
        public static Expression<Func<T,R>> ToGetter<T,R>(this string fieldName)
        {
            var parameter = Expression.Parameter(typeof(T), "p");

            return Expression.Lambda<Func<T, R>>(
                Expression.Property(parameter, fieldName),
                parameter);
        }
    }
}
