using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.AccessManagement.ResponsibilityTreeAccess.TOZ_sks;
internal abstract class SaveAccessAbstractTOZ<TEntity> : SaveAccessAbstract<TEntity>
    where TEntity : class
{
    private readonly IReadonlyRepository<Building> _buildings;
    private readonly IReadonlyRepository<Floor> _floors;
    private readonly IReadonlyRepository<Room> _rooms;

    protected override AccessTypes AccessType => AccessTypes.TOZ_sks;

    protected SaveAccessAbstractTOZ(IRepository<ObjectAccess> objectAccesses
        , IMapper mapper
        , IUnitOfWork unitOfWork
        , IReadonlyRepository<Building> buildings
        , IReadonlyRepository<Floor> floors
        , IReadonlyRepository<Room> rooms)
        : base(objectAccesses
            , mapper
            , unitOfWork)
    {
        _buildings = buildings;
        _floors = floors;
        _rooms = rooms;
    }

    #region Получение вложенных элементов
    protected async Task<Guid[]> GetOrganizationSubObjectsIDAsync(Guid organizationID, CancellationToken cancellationToken)
    {
        var subObjectID = new List<Guid>();

        var buildings = await _buildings.ToArrayAsync(c => c.OrganizationID == organizationID, cancellationToken);
        buildings.ForEach(c => subObjectID.Add(c.IMObjID));

        var buildingsID = buildings.Select(c => c.IMObjID).ToArray();
        subObjectID.AddRange(await GetBuildingSubObjectsIDAsync(buildingsID, cancellationToken));

        return subObjectID.ToArray();
    }

    protected async Task<Guid[]> GetBuildingSubObjectsIDAsync(Guid[] buildingsID, CancellationToken cancellationToken)
    {
        var subObjectID = new List<Guid>();

        var floors = await _floors.ToArrayAsync(c => buildingsID.Contains(c.Building.IMObjID) , cancellationToken);
        floors.ForEach(c => subObjectID.Add(c.IMObjID));

        var floorsID = floors.Select(c => c.IMObjID).ToArray();
        subObjectID.AddRange(await GetRoomsIDAsync(floorsID, cancellationToken));

        return subObjectID.ToArray();
    }

    protected async Task<Guid[]> GetRoomsIDAsync(Guid[] floorsID, CancellationToken cancellationToken)
    {
        var rooms = await _rooms.ToArrayAsync(c => floorsID.Contains(c.Floor.IMObjID), cancellationToken);

        return rooms.Select(c => c.IMObjID).ToArray();
    }
    #endregion

    #region Добавление вложенных элементов
    protected async Task InsertBuildingAccessAsync(Guid organizationID, Guid ownerID, CancellationToken cancellationToken)
    {
        var buildings = await _buildings.ToArrayAsync(c => c.OrganizationID == organizationID, cancellationToken);
        foreach (var building in buildings)
        {
            await InsertFloorAccessAsync(building.IMObjID, ownerID, cancellationToken);
            await InsertItemAsync(ownerID, building.IMObjID, ObjectClass.Building, cancellationToken: cancellationToken);
        }
    }

    protected async Task InsertFloorAccessAsync(Guid buildingID, Guid ownerID, CancellationToken cancellationToken)
    {
        var floors = await _floors.ToArrayAsync(c => c.Building.IMObjID == buildingID, cancellationToken);
        foreach (var floor in floors)
        {
            await InsertRoomAccessAsync(floor.IMObjID, ownerID, cancellationToken);
            await InsertItemAsync(ownerID, floor.IMObjID, ObjectClass.Floor, cancellationToken: cancellationToken);
        }
    }

    protected async Task InsertRoomAccessAsync(Guid floorID, Guid ownerID, CancellationToken cancellationToken)
    {
        var rooms = await _rooms.ToArrayAsync(c => c.Floor.IMObjID == floorID, cancellationToken);
        foreach (var room in rooms)
        {
            await InsertItemAsync(ownerID, room.IMObjID, ObjectClass.Room, cancellationToken: cancellationToken);
        }
    }
    #endregion
}
