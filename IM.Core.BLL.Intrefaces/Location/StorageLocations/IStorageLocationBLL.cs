using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Location.StorageLocations;

/// <summary>
/// Бизнес логика для складов
/// </summary>
public interface IStorageLocationBLL
{
    /// <summary>
    /// Получение дерева местоположения
    /// С условие что проводится проверка по расположение склада
    /// Если какой-то элемент ялвяется расположением склада, то IsSelectFull = true
    /// Если как минимум один из дочерних узлов дерева является расположением склада, то IsSelectPart = true
    /// Если не явл ни элемент, ни его дочерние, то элемент удаляется и не перадется на фронт
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="storageID"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<LocationTreeNodeDetails[]> GetTreeNodesByParentIDAsync(LocationTreeFilter filter, Guid storageID, CancellationToken cancellationToken);

    /// <summary>
    /// Обновление сущности вместе с ссылка на его местоположение
    /// </summary>
    /// <param name="model"></param>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StorageLocationDetails> UpdateAsync(StorageLocationDetails model, Guid id, CancellationToken cancellationToken);


    /// <summary>
    /// Добавление вместе с ссылками на доп сущности
    /// </summary>
    /// <param name="model"></param>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<StorageLocationDetails> AddAsync(StorageLocationInsertDetails model, CancellationToken cancellationToken);


    /// <summary>
    /// Удаление со всеми доп сущностями
    /// </summary>
    /// <param name="id">идентификатор удаляемой сущностиы</param>
    /// <param name="cancellationToken"></param>
    /// <returns>ничего не возвращает</returns>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Получение списка с возможностью скролинга, поиска и сортровки
    /// </summary>
    /// <param name="filter">фильтр местоположений</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Массив расположений склада, от расположение до старшего элемента соеденины через  _/_
    /// _ - пробел
    /// </returns>
    Task<LocationListItem[]> GetTableLocationAsync(StorageFilterLocation filter, CancellationToken cancellationToken);
}
