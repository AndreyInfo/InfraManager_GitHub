using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using InfraManager.BLL.Cloners;
using InfraManager.Expressions;
using InfraManager.Linq;
using Newtonsoft.Json;

namespace InfraManager.BLL.ServiceCatalogue.OperationalLevelAgreements;

//TODO: –азобратьс€ зачем это добавили и выпилить
public class Cloner<TSource> : ICloner<TSource>
{
    private readonly List<ClonerPropertyInfo> propertiesToChange = new();
    private readonly List<CollectionClonerPropertyInfo> collectionPropertiesToChange = new();


    public TSource Clone(TSource source)
    {
        if (source.Equals(null))
        {
            return default;
        }

        var deserializeSettings = new JsonSerializerSettings
            { ObjectCreationHandling = ObjectCreationHandling.Replace };
        
        var serializeSettings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };

        var clonedObject = JsonConvert.DeserializeObject<TSource>(JsonConvert.SerializeObject(source, serializeSettings),
            deserializeSettings);

        if (clonedObject == null)
        {
            throw new NotSupportedException($"Cant deserialize object of type {typeof(TSource)}");
        }

        foreach (var el in propertiesToChange)
        {
            ExpressionExtensions.ChangeValueWithExpression(clonedObject, el.ValueToSet, el.Property);
        }

        foreach (var el in collectionPropertiesToChange)
        {
            ExpressionExtensions.ChangeCollectionValueWithExpression(
                clonedObject,
                el.ValueToSet,
                el.CollectionToChange,
                el.PropertyToChange);
        }
        
        return clonedObject;
    }

    public ICloner<TSource> SetNewValueTo<TPropertyType>(Expression<Func<TSource, TPropertyType>> propertyToChange,
        TPropertyType newValue = default)
    {
        var newExpression =
            ExpressionExtensions.ConvertOutputExpressionResult<TSource, TPropertyType, object>(
                propertyToChange);
        propertiesToChange.Add(new ClonerPropertyInfo(newExpression, newValue));

        return this;
    }


    public ICloner<TSource> ForEachSetNewValueTo<TCollectionType, TCollectionPropertyType>(
        Expression<Func<TSource, ICollection<TCollectionType>>> collectionToChange,
        Expression<Func<TCollectionType, TCollectionPropertyType>> propertyToChange,
        TCollectionPropertyType newValue = default)
    {
        var newCollectionToChangeExpression =
            ExpressionExtensions.ConvertOutputExpressionResult<TSource, ICollection<TCollectionType>, ICollection<object>>(
                    collectionToChange);

        var propertyToChangeExpression =
            ExpressionExtensions
                .ConvertInputAndOutputExpressionType<TCollectionType, TCollectionPropertyType, object, object>(
                    propertyToChange);

        collectionPropertiesToChange.Add(new CollectionClonerPropertyInfo(newCollectionToChangeExpression,
            propertyToChangeExpression, newValue));

        return this;
    }


    private class ClonerPropertyInfo
    {
        public ClonerPropertyInfo(Expression<Func<TSource, object>> property,
            object valueToSet)
        {
            Property = property;
            ValueToSet = valueToSet;
        }
        
        public Expression<Func<TSource, object>> Property { get; }
        public object ValueToSet { get; }
    }

    private class CollectionClonerPropertyInfo
    {
        public CollectionClonerPropertyInfo(Expression<Func<TSource, ICollection<object>>> collectionToChange,
            Expression<Func<object, object>> propertyToChange,
            object valueToSet)
        {
            CollectionToChange = collectionToChange;
            PropertyToChange = propertyToChange;
            ValueToSet = valueToSet;
        }
        
        public Expression<Func<TSource, ICollection<object>>> CollectionToChange { get; }
        public Expression<Func<object, object>> PropertyToChange { get; }
        public object ValueToSet { get; }
    }
}