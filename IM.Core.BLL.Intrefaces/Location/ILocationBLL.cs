using System.Threading.Tasks;
using InfraManager.BLL.CrudWeb;
using System;
using System.Threading;

namespace InfraManager.BLL.Location;

/// <summary>
/// Реализует логику работы с деревом местоположений
/// С иерархией
/// Организация => Здание => Этаж => Комната => Шкаф или Рабочее место
/// </summary>
public interface ILocationBLL
{
    /// <summary>
    /// Получение уровня дерева местоположений, по фильтру
    /// </summary>
    /// <param name="model">филтр получения дерева</param>
    /// <param name="cancellationToken"></param>
    /// <returns>узлы дерева</returns>
    [Obsolete("Не использовать, фронт каждый уровень должен получать через отдельное API")]
    Task<LocationTreeNodeDetails[]> GetLocationNodesByParentIdAndRightsUserAsync(LocationTreeFilter model, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение ветки дерева, от дочернего до родительского
    /// </summary>
    /// <param name="classID">тип объекта</param>
    /// <param name="id">индетификатор объекта от которого берем ветку</param>
    /// <param name="cancellationToken"></param>
    /// <returns>узлы дерева</returns>
    Task<LocationTreeNodeDetails[]> GetBranchTreeAsync(ObjectClass classID, Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение узлов дерева местоположений, уровня владельцев
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>узлы владельцев</returns>
    Task<LocationTreeNodeDetails[]> GetOwnerNodesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Получение узлов дерева местоположений, уровня организаций
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>узлы организаций</returns>
    Task<LocationTreeNodeDetails[]> GetOrganizationNodesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Получение узлов дерева местоположений, уровня зданий
    /// </summary>
    /// <param name="organizationID">идентификатор организации</param>
    /// <param name="cancellationToken"></param>
    /// <returns>узлы зданий</returns>
    Task<LocationTreeNodeDetails[]> GetBuildingNodesAsync(Guid organizationID, CancellationToken cancellationToken);

    /// <summary>
    /// Получение узлов дерева местоположений, уровня этажей
    /// </summary>
    /// <param name="buildingID">идентификатор здания</param>
    /// <param name="cancellationToken"></param>
    /// <returns>узлы этажей</returns>
    Task<LocationTreeNodeDetails[]> GetFloorNodesAsync(int buildingID, CancellationToken cancellationToken);

    /// <summary>
    /// Получение узлов дерева местоположений, уровня комнат
    /// </summary>
    /// <param name="floorID">идентификатор этажа</param>
    /// <param name="childClassID">тип объектов доерних узлов, для определния есть ниже уровнем узлы по бизнес логике</param>
    /// <param name="cancellationToken"></param>
    /// <returns>узлы комнат</returns>
    Task<LocationTreeNodeDetails[]> GetRoomNodesAsync(int floorID, ObjectClass? childClassID, CancellationToken cancellationToken);

    /// <summary>
    /// Получение узлов дерева местоположений, уровня рабочих мест
    /// </summary>
    /// <param name="roomID">идентификатор комнаты</param>
    /// <param name="cancellationToken"></param>
    /// <returns>узлы рабочих мест</returns>
    Task<LocationTreeNodeDetails[]> GetWorkplaceNodesAsync(int roomID, CancellationToken cancellationToken);

    /// <summary>
    /// Получение узлов дерева местоположений, уровня комнат
    /// </summary>
    /// <param name="roomID">идентификатор комнаты</param>
    /// <param name="cancellationToken"></param>
    /// <returns>узлы шкафов</returns>
    Task<LocationTreeNodeDetails[]> GetRackNodesAsync(int roomID, CancellationToken cancellationToken);

}
