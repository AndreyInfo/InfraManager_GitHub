using AutoMapper;
using Inframanager.BLL;
using InfraManager.Linq;
using System;
using System.Linq.Expressions;

namespace InfraManager.BLL.AutoMapper
{
    internal static class MappingExpressionExtensions
    {
        public static void IgnoreNulls<TSource, TDest>(
            this IMappingExpression<TSource, TDest> mapping)
        {
            mapping.ForAllMembers(IgnoreNullsCondition<TSource, TDest>());
        }

        public static IMappingExpression<TSource, TDest> IgnoreOtherNulls<TSource, TDest>(
            this IMappingExpression<TSource, TDest> mapping)
        {
            mapping.ForAllOtherMembers(IgnoreNullsCondition<TSource, TDest>());
            return mapping; 
        }

        private static Action<IMemberConfigurationExpression<TSource, TDest, object>> IgnoreNullsCondition<TSource, TDest>() =>
            opts => opts.Condition((src, dest, srcMember, targetMember) => srcMember != null);

        public static IMappingExpression<TData, TEntity> ForNullableProperty<TData, TEntity, T>(
            this IMappingExpression<TData, TEntity> mappingBuilder,
            Expression<Func<TData, NullablePropertyWrapper<T>>> sourceExpression,
            Expression<Func<TEntity, Nullable<T>>> targetExpression) where T : struct
        {
            return mappingBuilder.ForMember(
                targetExpression,
                mapper => mapper.MapFrom(sourceExpression));
        }

        public static IMappingExpression<TData, TEntity> ForNullableProperty<TData, TEntity, T>(
            this IMappingExpression<TData, TEntity> mappingBuilder,
            Expression<Func<TData, NullablePropertyWrapper<T>>> sourceExpression) where T : struct
        {
            var sourceProperty = sourceExpression.TryGetPropertyName(out var propertyName)
                ? propertyName
                : throw new ArgumentException("MemberExpression expected", nameof(sourceExpression));
            var expressionParameter = Expression.Parameter(typeof(TEntity));
            var targetExpression = Expression.Lambda<Func<TEntity, Nullable<T>>>(
                Expression.Property(expressionParameter, sourceProperty),
                expressionParameter);

            return mappingBuilder.ForNullableProperty(sourceExpression, targetExpression);
        }
    }
}
