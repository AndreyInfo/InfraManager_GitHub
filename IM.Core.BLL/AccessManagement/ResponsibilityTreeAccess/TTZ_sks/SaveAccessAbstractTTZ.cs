using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.Asset;
using InfraManager.DAL.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.AccessManagement.ResponsibilityTreeAccess.TTZ_sks;
internal abstract class SaveAccessAbstractTTZ<TEntity> : SaveAccessAbstract<TEntity>
    where TEntity : class
{
    private readonly IReadonlyRepository<Building> _buildings;
    private readonly IReadonlyRepository<Floor> _floors;
    private readonly IReadonlyRepository<Room> _rooms;
    private  readonly IReadonlyRepository<Rack> _racks;

    protected override AccessTypes AccessType => AccessTypes.TTZ_sks;

    protected SaveAccessAbstractTTZ(IRepository<ObjectAccess> objectAccesses
        , IMapper mapper
        , IUnitOfWork unitOfWork
        , IReadonlyRepository<TEntity> entities
        , IReadonlyRepository<Building> buildings
        , IReadonlyRepository<Floor> floors
        , IReadonlyRepository<Room> rooms
        , IReadonlyRepository<Rack> racks)
        : base(objectAccesses
            , mapper
            , unitOfWork)
    {
        _buildings = buildings;
        _floors = floors;
        _rooms = rooms;
        _racks = racks;
    }

    #region Получение вложенных элементов
    protected async Task<Guid[]> GetOrganizationSubObjectsIDAsync(Guid organizationID, CancellationToken cancellationToken)
    {
        var subObjectID = new List<Guid>();

        var buildings = await _buildings.ToArrayAsync(c => c.OrganizationID == organizationID, cancellationToken);
        buildings.ForEach(c => subObjectID.Add(c.IMObjID));

        var buildingsID = buildings.Select(c => c.IMObjID).ToArray();
        subObjectID.AddRange(await GetBuildingsSubObjectsIDAsync(buildingsID, cancellationToken));

        return subObjectID.ToArray();
    }

    protected async Task<Guid[]> GetBuildingsSubObjectsIDAsync(Guid[] buildingsID, CancellationToken cancellationToken)
    {
        var subObjectID = new List<Guid>();

        var rooms = await _floors.ToArrayAsync(c => buildingsID.Contains(c.Building.IMObjID) , cancellationToken);
        rooms.ForEach(c => subObjectID.Add(c.IMObjID));

        var floorsID = rooms.Select(c => c.IMObjID).ToArray();
        subObjectID.AddRange(await GetFloorsSubObjectsIDAsync(floorsID, cancellationToken));

        return subObjectID.ToArray();
    }

    protected async Task<Guid[]> GetFloorsSubObjectsIDAsync(Guid[] floorsID, CancellationToken cancellationToken)
    {
        var subObjectID = new List<Guid>();

        var rooms = await _rooms.ToArrayAsync(c => floorsID.Contains(c.Floor.IMObjID), cancellationToken);
        rooms.ForEach(c => subObjectID.Add(c.IMObjID));

        var roomsID = rooms.Select(c => c.IMObjID).ToArray();
        subObjectID.AddRange(await GetRacksIDAsync(roomsID, cancellationToken));

        return subObjectID.ToArray();
    }

    protected async Task<Guid[]> GetRacksIDAsync(Guid[] roomIDs, CancellationToken cancellationToken)
    {
        var subObjectID = new List<Guid>();

        var racks = await _racks.ToArrayAsync(c => roomIDs.Contains(c.IMObjID), cancellationToken);
        racks.ForEach(c => subObjectID.Add(c.IMObjID));

        return subObjectID.ToArray();
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
            await InsertRackAccessAsync(room.IMObjID, ownerID, cancellationToken);
            await InsertItemAsync(ownerID, room.IMObjID, ObjectClass.Room, cancellationToken: cancellationToken);
        }
    }

    protected async Task InsertRackAccessAsync(Guid roomID, Guid ownerID, CancellationToken cancellationToken)
    {
        var racks = await _racks.ToArrayAsync(c => c.Room.IMObjID == roomID, cancellationToken);
        foreach (var rack in racks)
        {
            await InsertItemAsync(ownerID, rack.IMObjID, ObjectClass.Rack, cancellationToken: cancellationToken);
        }
    }
    #endregion
}
