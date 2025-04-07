using System;
using InfraManager.Linq;
using System.Linq;

namespace InfraManager
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Where<T>(this IQueryable<T> source, string propertyName, ComparisonType comparison, string value)
        {
            return source.Where(ExpressionExtensions.BuildPredicate<T>(propertyName, comparison, value));
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, bool ascending)
        {
            var propertyExpression = ExpressionExtensions.Property<T, Object>(propertyName);

            return ascending 
                ? source.OrderBy(propertyExpression) 
                : source.OrderByDescending(propertyExpression);
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, Sort sortParameters)
        {
            return source.OrderBy(sortParameters.PropertyName, sortParameters.Ascending);
        }
        
        public static IOrderedQueryable<T> ThenOrderBy<T>(this IOrderedQueryable<T> source, string propertyName, bool ascending)
        {
            var propertyExpression = ExpressionExtensions.Property<T, Object>(propertyName);

            return ascending 
                ? source.ThenBy(propertyExpression) 
                : source.ThenByDescending(propertyExpression);
        }

        public static IOrderedQueryable<T> ThenOrderBy<T>(this IOrderedQueryable<T> source, Sort sortParameters)
        {
            return source.ThenOrderBy(sortParameters.PropertyName, sortParameters.Ascending);
        }
    }
}
