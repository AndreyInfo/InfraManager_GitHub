using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.Asset;
using InfraManager.DAL.Location;
using InfraManager.DAL.OrganizationStructure;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.AccessManagement.ResponsibilityTreeAccess.TTZ_sks;
internal sealed class SaveAccessOwnerTTZ : SaveAccessAbstract<Owner>
{
    private readonly IReadonlyRepository<Organization> _organizations;
    private readonly IReadonlyRepository<Building> _buildings;
    private readonly IReadonlyRepository<Floor> _floors;
    private readonly IReadonlyRepository<Room> _rooms;
    private readonly IReadonlyRepository<Rack> _racks;

    protected override AccessTypes AccessType => AccessTypes.TTZ_sks;
    protected override ObjectClass ClassID => ObjectClass.Owner;
    protected override IReadOnlyCollection<ObjectClass> ChildClasses => new ObjectClass[] 
    { 
        ObjectClass.Organizaton,
        ObjectClass.Building,
        ObjectClass.Floor,
        ObjectClass.Room,
        ObjectClass.Rack,
    };

    public SaveAccessOwnerTTZ(IRepository<ObjectAccess> objectAccesses
        , IMapper mapper
        , IUnitOfWork unitOfWork
        , IReadonlyRepository<Organization> organizations
        , IReadonlyRepository<Building> buildings
        , IReadonlyRepository<Floor> floors
        , IReadonlyRepository<Room> rooms
        , IReadonlyRepository<Rack> racks)
        : base(objectAccesses
            , mapper
            , unitOfWork)
    {
        _organizations = organizations;
        _buildings = buildings;
        _floors = floors;
        _rooms = rooms;
        _racks = racks;
    }


    #region Insert SubObjects
    protected override async Task InsertSubObjectsAsync(Guid parentID, Guid ownerID, CancellationToken cancellationToken)
    {
        await InsertAllOrganizationAccessAsync(ownerID, cancellationToken);
        await InsertLocationAccessAsync(_buildings, ownerID, ObjectClass.Building, cancellationToken);
        await InsertLocationAccessAsync(_floors, ownerID, ObjectClass.Floor, cancellationToken);
        await InsertLocationAccessAsync(_rooms, ownerID, ObjectClass.Room, cancellationToken);
        await InsertAllRackAccessAsync(ownerID, cancellationToken);
    }

    private async Task InsertAllOrganizationAccessAsync(Guid ownerID, CancellationToken cancellationToken)
    {
        var organizations = await _organizations.ToArrayAsync(cancellationToken);
        foreach (var organization in organizations)
            await InsertItemAsync(ownerID, organization.ID, ObjectClass.Organizaton, cancellationToken: cancellationToken);
    }

    private async Task InsertLocationAccessAsync<T>(IReadonlyRepository<T> repository
        , Guid ownerID
        , ObjectClass classID
        , CancellationToken cancellationToken)
        where T : LocationObject
    {
        var locations = await repository.ToArrayAsync(cancellationToken);
        foreach (var location in locations)
        {
            var locationObjectID = GetLocationObjectID(location);
            await InsertItemAsync(ownerID, locationObjectID, classID, cancellationToken: cancellationToken);
        }
    }

    private async Task InsertAllRackAccessAsync(Guid ownerID, CancellationToken cancellationToken)
    {
        var racks = await _racks.ToArrayAsync(cancellationToken);
        foreach (var rack in racks)
            await InsertItemAsync(ownerID, rack.IMObjID, ObjectClass.Rack, cancellationToken: cancellationToken);
    }

    protected override Task<Guid[]> GetIDsSubObjectsAsync(Guid parentID, CancellationToken cancellationToken)
    {
        return Task.FromResult(Array.Empty<Guid>());
    }

    private Guid? GetLocationObjectID<T>(T location)
    {
        return location switch
        {
            Building building => (Guid?)building.IMObjID,
            Floor floor => (Guid?)floor.IMObjID,
            Room room => (Guid?)room.IMObjID,
            Rack rack => rack.IMObjID,
            _ => null,
        };
    }

    #endregion
}
