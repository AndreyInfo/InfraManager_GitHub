using InfraManager.Expressions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace InfraManager.Linq
{
    /// <summary>
    /// Расширение для  Expression
    /// </summary>
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> BuildPredicate<T>(
            this Expression<Func<T, object>> property,
            ComparisonType comparison,
            string value)
        {
            var parameter = property.Parameters.First();
            var body = property.Body.MakeComparison(comparison, value);

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> BuildPredicate<T>(string propertyName, ComparisonType comparison, string value)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var left = propertyName.Split('.').Aggregate((Expression)parameter, Expression.Property);
            var body = MakeComparison(left, comparison, value);
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static PropertyInfo ExtractPropertyInfo<T, TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            if (propertyExpression.Body is UnaryExpression unaryExpression)
                return (PropertyInfo)((MemberExpression)unaryExpression.Operand).Member;

            MemberExpression memberExpression = (MemberExpression)propertyExpression.Body;

            return (PropertyInfo)memberExpression.Member;
        }

        private static Expression MakeComparison(this Expression left, ComparisonType comparison, string value)
        {
            switch (comparison)
            {
                case ComparisonType.Equal:
                    return MakeBinary(ExpressionType.Equal, left, value);
                case ComparisonType.NotEqual:
                    return MakeBinary(ExpressionType.NotEqual, left, value);
                case ComparisonType.GreaterThan:
                    return MakeBinary(ExpressionType.GreaterThan, left, value);
                case ComparisonType.GreaterThanOrEqual:
                    return MakeBinary(ExpressionType.GreaterThanOrEqual, left, value);
                case ComparisonType.LessThan:
                    return MakeBinary(ExpressionType.LessThan, left, value);
                case ComparisonType.LessThanOrEqual:
                    return MakeBinary(ExpressionType.LessThanOrEqual, left, value);
                case ComparisonType.Contains:
                    return Expression.Call(MakeString(left), "Contains", Type.EmptyTypes, Expression.Constant(value, typeof(string)));
                case ComparisonType.StartsWith:
                    return Expression.Call(MakeString(left), "StartsWith", Type.EmptyTypes, Expression.Constant(value, typeof(string)));
                case ComparisonType.EndsWith:
                    return Expression.Call(MakeString(left), "EndsWith", Type.EmptyTypes, Expression.Constant(value, typeof(string)));
                case ComparisonType.NotContains:
                    return Expression.Not(MakeComparison(left, ComparisonType.Contains, value));
                default:
                    throw new NotSupportedException($"Invalid comparison operator '{comparison}'.");
            }
        }

        private static Expression MakeString(this Expression source)
        {
            return source.Type == typeof(string) ? source : Expression.Call(source, "ToString", Type.EmptyTypes);
        }

        private static Expression MakeBinary(ExpressionType type, Expression left, string value)
        {
            object typedValue = value;
            if (left.Type != typeof(string))
            {
                if (string.IsNullOrEmpty(value))
                {
                    typedValue = null;
                    if (Nullable.GetUnderlyingType(left.Type) == null)
                        left = Expression.Convert(left, typeof(Nullable<>).MakeGenericType(left.Type));
                }
                else
                {
                    var valueType = Nullable.GetUnderlyingType(left.Type) ?? left.Type;
                    typedValue = valueType.IsEnum ? Enum.Parse(valueType, value) :
                        valueType == typeof(Guid) ? Guid.Parse(value) :
                        Convert.ChangeType(value, valueType);
                }
            }
            var right = Expression.Constant(typedValue, left.Type);
            return Expression.MakeBinary(type, left, right);
        }

        public static Expression<Func<T, TResult>> PropertyPath<T, TResult>(string path)
        {
            var parameter = Expression.Parameter(typeof(T));
            return Expression.Lambda<Func<T, TResult>>(
                PropertyPath(path.Split('.'), parameter),
                parameter);
        }

        private static Expression PropertyPath(string[] parts, Expression expression)
        {
            var next = Expression.Property(expression, parts[0]);

            return parts.Length == 1 ? next : PropertyPath(parts.Skip(1).ToArray(), next);
        }

        public static Expression<Func<T, TResult>> Property<T, TResult>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            Expression body = Expression.Property(parameter, propertyName);

            var property = typeof(T).GetProperty(propertyName);
            if (typeof(TResult) != property?.PropertyType)
            {
                body = Expression.Convert(body, typeof(TResult));
            }

            return Expression.Lambda<Func<T, TResult>>(body, parameter);
        }

        public static bool TryGetPropertyName<T, V>(this Expression<Func<T, V>> expression, out string propertyName)
        {
            var memberExpression = expression.Body as MemberExpression;
            propertyName = memberExpression?.Member?.Name;

            return !string.IsNullOrEmpty(propertyName);
        }

        public static IQueryable<T> AddFilter<T, Type>(IQueryable<T> query, string propertyName, Type search, ExpressionType type)
        {
            var param = Expression.Parameter(typeof(T), "e");
            var propExpression = Expression.Property(param, propertyName);

            object value = search;

            var filter = MakeFilter(propExpression, value, type);

            var filterLambda = Expression.Lambda<Func<T, bool>>(filter, param);

            return query.Where(filterLambda);
        }

        private static Expression MakeFilter(MemberExpression prop, object searchTerm, ExpressionType type)
        {
            return Expression.MakeBinary(type, prop, Expression.Constant(searchTerm));
        }

        public static Expression<Func<T, bool>> And<T>(
            this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            if (left == null)
            {
                return right ?? throw new ArgumentNullException(nameof(left), "Left expression is null");
            }

            if (right == null)
            {
                return left;
            }

            var rightBody = new ExpressionReplacer(right.Parameters[0], left.Parameters[0]).Visit(right.Body);
            return Expression.Lambda<Func<T, bool>>(Expression.And(left.Body, rightBody), left.Parameters[0]);
        }

        public static Expression<Func<T, bool>> And<T>(
            this IEnumerable<Expression<Func<T, bool>>> left, Expression<Func<T, bool>> right)
        {
            if (left == null)
            {
                return right ?? throw new ArgumentNullException(nameof(left), "Left expression is null");
            }

            return left.Intersect().And(right);
        }

        public static Expression<Func<T1, T2>> Or<T1, T2>(this Expression<Func<T1, T2>> left, Expression<Func<T1, T2>> right)
        {
            if (left == null)
            {
                return right ?? throw new ArgumentNullException(nameof(left), "Left expression is null");
            }

            if (right == null)
            {
                return left;
            }

            var rightBody = new ExpressionReplacer(right.Parameters[0], left.Parameters[0]).Visit(right.Body);
            return Expression.Lambda<Func<T1, T2>>(Expression.OrElse(left.Body, rightBody), left.Parameters[0]);
        }

        public static Expression<Func<DateTime, bool>> DateTimeBetween(DateTime? start, DateTime? finish, bool onlyDate = false) // TODO: use precision instead "onlyDate"
        {
            var minDate = onlyDate ? start.Value.Date : start.Value.TruncateSeconds();
            var maxDate = onlyDate ? finish.Value.Date.AddDays(1) : finish.Value.TruncateSeconds().AddSeconds(1);

            return dt => dt > minDate && dt <= maxDate;
        }

        public static Expression<Func<DateTime?, bool>> NullableDateTimeBetween(DateTime? start, DateTime? finish, bool onlyDate = false)
        {
            var minDate = onlyDate ? start.Value.Date : start.Value.TruncateSeconds();
            var maxDate = onlyDate ? finish.Value.Date.AddDays(1) : finish.Value.TruncateSeconds().AddSeconds(1);

            return dt => dt > minDate && dt <= maxDate;
        }

        public static string GetPropertyName<TPropertySource>
            (this Expression<Func<TPropertySource, object>> expression)
        {
            var lambda = expression as LambdaExpression;
            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = lambda.Body as UnaryExpression;
                memberExpression = unaryExpression?.Operand as MemberExpression;
            }
            else
            {
                memberExpression = lambda.Body as MemberExpression;
            }

            if (memberExpression != null)
            {
                var propertyInfo = memberExpression.Member as PropertyInfo;

                return propertyInfo?.Name;
            }

            return null;
        }

        public static Expression<Func<T, object>> WithNestedProperty<T>(string columnName)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "x");

            // x.ColumnName1.ColumnName2
            Expression property = columnName.Split('.')
                .Aggregate<string, Expression>
                    (param, (c, m) => Expression.Property(c, m));

            // x => x.ColumnName1.ColumnName2
            return Expression.Lambda<Func<T, object>>(property, param);
        }

        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> initialExpression)
        {
            return Expression.Lambda<Func<T, bool>>(
                Expression.Not(initialExpression.Body),
                initialExpression.Parameters);
        }

        public static IEnumerable<string> PropertyPathRecurse<TModel, TValue>(Expression<Func<TModel, TValue>> expression)
        {
            if (expression is null)
                return Enumerable.Empty<string>();

            if (expression.Body is MemberExpression memberExpression)
            {
                return PropertyPathRecurse(memberExpression.Expression as MemberExpression)
                    .Append(memberExpression.Member.Name);
            }

            if (expression.Body is UnaryExpression unaryExpression)
            {
                return new[] { ((unaryExpression.Operand as MemberExpression)?.Member as PropertyInfo)?.Name };
            }

            return Enumerable.Empty<string>();
        }

        private static IEnumerable<string> PropertyPathRecurse(MemberExpression? expression)
        {
            if (expression is null)
                return Enumerable.Empty<string>();

            return PropertyPathRecurse(expression.Expression as MemberExpression)
                .Append(expression.Member.Name);
        }

        public static void ChangeValueWithExpression<ObjectType, PropertyType>(ObjectType value,
            PropertyType changeValue,
            Expression<Func<ObjectType, PropertyType>> expression)
        {
            var propertyInfo = ExtractPropertyInfo(expression);

            SetValueToProperty(propertyInfo, value, changeValue);
        }

        public static void ChangeCollectionValueWithExpression<ObjectType, PropertyType,TChangeProperty>(ObjectType value,
            TChangeProperty changeValue,
            Expression<Func<ObjectType, ICollection<PropertyType>>> expression,
            Expression<Func<PropertyType, TChangeProperty>> changeProperty)
        {
            var propertyInfo = ExtractPropertyInfo(expression);

            var elements = (IEnumerable<PropertyType>)propertyInfo.GetValue(value);

            if (elements == null)
            {
                return;
            }
            
            var changePropertyInfo = ExtractPropertyInfo(changeProperty);

            foreach (var el in elements)
            {
                SetValueToProperty(changePropertyInfo, el, changeValue);
            }
        }

        private static void SetValueToProperty<TChangeObject, TChangeValue>(PropertyInfo propertyInfo, TChangeObject @object,
            TChangeValue value)
        {
            if (propertyInfo.CanWrite)
            {
                propertyInfo.SetValue(@object, value, null);
            }
        }

        public static Expression<Func<T, TNew>> ConvertOutputExpressionResult<T, TOriginal, TNew>(
            Expression<Func<T, TOriginal>> expression)
        {
            var convertedBody = Expression.Convert(expression.Body, typeof(TNew));

            return Expression.Lambda<Func<T, TNew>>(
                convertedBody,
                expression.Parameters.First());
        }

        public static Expression<Func<TIn, TNew>> ConvertInputAndOutputExpressionType<T, TOriginal, TIn, TNew>(
            Expression<Func<T, TOriginal>> expression)
        {
            var convertedBody = Expression.Convert(expression.Body, typeof(TNew));

            var parameter = Expression.Parameter(typeof(TIn), "x");
            return Expression.Lambda<Func<TIn, TNew>>(
                convertedBody,
                parameter);
        }

        public static Expression<Func<TInitial, TResult>> Substitute<TInitial, T, TResult>(
            this Expression<Func<T, TResult>> target, 
            Expression<Func<TInitial, T>> substitution)
        {
            var body = new ExpressionReplacer(target.Parameters[0], substitution.Body).Visit(target.Body);

            return Expression.Lambda<Func<TInitial, TResult>>(body, substitution.Parameters[0]);
        }

        public static Expression<Func<T, bool>> Intersect<T>(this IEnumerable<Expression<Func<T, bool>>> predicates)
        {
            if (!predicates.Any())
            {
                return x => true;
            }

            var result = predicates.First();
            foreach(var nextPredicate in predicates.Skip(1))
            {
                result = result.And(nextPredicate);
            }

            return result;
        }
    }
}