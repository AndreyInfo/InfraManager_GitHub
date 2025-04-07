using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace InfraManager.DAL;

public interface ISearchTextPredicateBuilder
{
    /// <summary>
    /// Построить предикат поиска по заданному текстовому свойству.
    /// </summary>
    /// <param name="property">Свойство, по которому поиск.</param>
    /// <param name="text">Текст, который ищем.</param>
    /// <param name="likePattern">Тип сравнения.</param>
    /// <typeparam name="TEntity">Тип сущности.</typeparam>
    /// <returns>Лямбда-выражение.</returns>
    Expression<Func<TEntity, bool>> BuildPredicate<TEntity>(
        Expression<Func<TEntity, string>> property,
        string text,
        LikePatterns likePattern = LikePatterns.Contains);

    /// <summary>
    /// Построить предикат поиска по заданным текстовым свойствам.
    /// Для каждого поля создает LIKE-выражение. Результат агрегируется через OR.
    /// </summary>
    /// <param name="properties">Свойства, по которым поиск.</param>
    /// <param name="text">Текст, который ищем.</param>
    /// <param name="likePattern">Тип сравнения.</param>
    /// <typeparam name="TEntity">Тип сущности.</typeparam>
    /// <returns>Лямбда-выражение.</returns>
    Expression<Func<TEntity, bool>> BuildPredicate<TEntity>(
        IEnumerable<Expression<Func<TEntity, string>>> properties,
        string text,
        LikePatterns likePattern = LikePatterns.Contains);
}