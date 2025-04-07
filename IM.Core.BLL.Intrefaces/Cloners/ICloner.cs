using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace InfraManager.BLL.Cloners;

public interface ICloner<TSource>
{
    /// <summary>
    /// Возврощает копию объекта <see cref="TSource"/>
    /// </summary>
    /// <param name="source">Объект который нужно скопировать</param>
    /// <returns></returns>
    TSource Clone(TSource source);

    /// <summary>
    /// Заменяет значения у выбранного свойства у клонируемого объекта
    /// </summary>
    /// <param name="propertyToChange">Свойство, которое нужно заменить</param>
    /// <param name="newValue">Значение, на которое нужно заменить</param>
    /// <typeparam name="TPropertyType">Тип заменяемого свойства</typeparam>
    ICloner<TSource> SetNewValueTo<TPropertyType>(Expression<Func<TSource, TPropertyType>> propertyToChange,
        TPropertyType newValue = default);

    /// <summary>
    /// Заменяет свойство у каждого элемента в выбранной коллекции
    /// </summary>
    /// <param name="collectionToChange">Коллекция объектов, в которых нужно заменить свойство</param>
    /// <param name="propertyToChange">Свойство для замены</param>
    /// <param name="newValue">Новое значение</param>
    /// <typeparam name="TCollectionType">Тип коллекции</typeparam>
    /// <typeparam name="TCollectionPropertyType">Тип свойства</typeparam>
    ICloner<TSource> ForEachSetNewValueTo<TCollectionType, TCollectionPropertyType>(
        Expression<Func<TSource, ICollection<TCollectionType>>> collectionToChange,
        Expression<Func<TCollectionType, TCollectionPropertyType>> propertyToChange,
        TCollectionPropertyType newValue = default);
}