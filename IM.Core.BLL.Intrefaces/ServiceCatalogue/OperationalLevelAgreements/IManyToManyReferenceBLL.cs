using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL;

namespace InfraManager.BLL.ServiceCatalogue.OperationalLevelAgreements;

public interface IManyToManyReferenceBLL<TParent,TKey> where TParent : class
{
    /// <summary>
    /// Добавляет <see cref="TReference"/> к коллекции в <see cref="TParent"/>
    /// </summary>
    /// <param name="collectionExpression">Коллекция, в которую добавляется дочерняя сущность</param>
    /// <param name="id">Идентификатор <see cref="TParent"/></param>
    /// <param name="finder">Делегат получения добавляемой дочерней сущности</param>
    /// <param name="referenceID">Ключ добавляемой дочерней сущности</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <typeparam name="TReference">Тип добавляемой дочерней сущности</typeparam>
    /// <typeparam name="TReferenceKey">Тип ключа добавляемой дочерней сущности</typeparam>
    Task<ManyToMany<TParent, TReference>> AddReferenceAsync<TReference, TReferenceKey>(
        Expression<Func<TParent, ICollection<ManyToMany<TParent, TReference>>>> collectionExpression,
        TKey id,
        Func<TReferenceKey, Task<TReference>> finder,
        TReferenceKey referenceID,
        CancellationToken cancellationToken = default) where TReference : class;

    /// <summary>
    /// Удаляет <see cref="TReference"/> из коллекции в <see cref="TParent"/>
    /// </summary>
    /// <param name="id">Идентификатор <see cref="TParent"/></param>
    /// <param name="referenceID">Ключ удаляемой дочерней сущности</param>
    /// <param name="collectionExpression">Коллекция, из которой удаляется дочерняя сущность</param>
    /// <param name="referenceIDAccessor">Делегат получения удаляемой дочерней сущности</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <typeparam name="TReference">Тип удаляемой дочерней сущности</typeparam>
    /// <typeparam name="TReferenceKey">Тип ключа удаляемой дочерней сущности</typeparam>
    Task RemoveReferenceAsync<TReference, TReferenceKey>(
        TKey id,
        TReferenceKey referenceID,
        Expression<Func<TParent, ICollection<ManyToMany<TParent, TReference>>>> collectionExpression,
        Func<TReference, TReferenceKey> referenceIDAccessor,
        CancellationToken cancellationToken = default) where TReference : class where TReferenceKey : struct;

    /// <summary>
    /// Получение списка дочерних сущностей
    /// </summary>
    /// <param name="id">ID объекта, из которого будет получен список дочерних сущностей</param>
    /// <param name="collectionExpression">Коллекция которая будет возвращена</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <typeparam name="TReference">Тип получаемой дочерней сущности</typeparam>
    Task<TReference[]> GetReferencesAsync<TReference>(
        TKey id,
        Expression<Func<TParent, ICollection<ManyToMany<TParent, TReference>>>> collectionExpression,
        CancellationToken cancellationToken = default) where TReference : class;
}