using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.Asset;
using InfraManager.DAL.Location;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.AccessManagement.ResponsibilityTreeAccess.TTZ_sks;
internal sealed class SaveAccessRoomTTZ : SaveAccessAbstractTTZ<Room>
{
    protected override ObjectClass ClassID => ObjectClass.Room;
    protected override IReadOnlyCollection<ObjectClass> ChildClasses => new ObjectClass[] 
    { 
        ObjectClass.Rack
    };

    public SaveAccessRoomTTZ(IRepository<ObjectAccess> objectAccesses
        , IMapper mapper
        , IUnitOfWork unitOfWork
        , IReadonlyRepository<Building> buildings
        , IReadonlyRepository<Floor> floors
        , IReadonlyRepository<Room> rooms
        , IReadonlyRepository<Rack> racks)
        : base(objectAccesses
            , mapper
            , unitOfWork
            , rooms
            , buildings
            , floors
            , rooms
            , racks)
    {
    }


    protected override async Task InsertSubObjectsAsync(Guid parentID, Guid ownerID, CancellationToken cancellationToken) 
        => await InsertRackAccessAsync(parentID, ownerID, cancellationToken);

    protected override async Task<Guid[]> GetIDsSubObjectsAsync(Guid parentID, CancellationToken cancellationToken) 
        => await GetRacksIDAsync(new Guid[] { parentID }, cancellationToken);
}
