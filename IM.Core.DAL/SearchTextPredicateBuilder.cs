using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using InfraManager.Linq;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL;

internal class SearchTextPredicateBuilder : ISearchTextPredicateBuilder, ISelfRegisteredService<ISearchTextPredicateBuilder>
{
    public Expression<Func<TEntity, bool>> BuildPredicate<TEntity>(
        Expression<Func<TEntity, string>> property,
        string text,
        LikePatterns likePattern = LikePatterns.Contains)
    {
        _ = property ?? throw new ArgumentNullException(nameof(property));

        var pattern = MakePattern(text, likePattern);

        return Expression.Lambda<Func<TEntity, bool>>(
            MakeEFLikeCallExpression(property, pattern),
            property.Parameters);
    }

    public Expression<Func<TEntity, bool>> BuildPredicate<TEntity>(
        IEnumerable<Expression<Func<TEntity, string>>> properties,
        string text,
        LikePatterns likePattern = LikePatterns.Contains)
    {
        var expressions = properties as Expression<Func<TEntity, string>>[] ?? properties.ToArray();
        if (expressions.Length == 0)
        {
            throw new ArgumentNullException(nameof(properties));
        }
        
        var pattern = MakePattern(text, likePattern);

        var parameter = expressions.First().Parameters[0];
        var body = Expression.Lambda<Func<TEntity, bool>>(Expression.Constant(false), parameter);

        var result = expressions
            .Select(property => Expression.Lambda<Func<TEntity, bool>>(
                MakeEFLikeCallExpression(property, pattern), 
                property.Parameters[0]))
            .Aggregate(body, (current, aa) => current.Or(aa));

        return result;
    }

    private static MethodCallExpression MakeEFLikeCallExpression<TEntity>(Expression<Func<TEntity, string>> property, string pattern)
    {
        return Expression.Call(
            typeof(DbFunctionsExtensions),
            nameof(DbFunctionsExtensions.Like),
            null,
            Expression.Constant(EF.Functions),
            property.Body,
            Expression.Constant(pattern));
    }

    private static string MakePattern(string text, LikePatterns likePattern)
    {
        switch (likePattern)
        {
            case LikePatterns.Contains:
                return text.ToContainsPattern();

            case LikePatterns.StartsWith:
                return text.ToStartsWithPattern();

            case LikePatterns.EndsWith:
                return text.ToEndsWithPattern();

            default:
                return text;
        }
    }
}