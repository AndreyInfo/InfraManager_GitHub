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
internal sealed class SaveAccessFloorTTZ : SaveAccessAbstractTTZ<Floor>
{
    protected override ObjectClass ClassID => ObjectClass.Floor;
    protected override IReadOnlyCollection<ObjectClass> ChildClasses => new ObjectClass[] 
    {
        ObjectClass.Room,
        ObjectClass.Rack,
    };


    public SaveAccessFloorTTZ(IRepository<ObjectAccess> objectAccesses
        , IMapper mapper
        , IUnitOfWork unitOfWork
        , IReadonlyRepository<Building> buildings
        , IReadonlyRepository<Floor> floors
        , IReadonlyRepository<Room> rooms
        , IReadonlyRepository<Rack> racks)
        : base(objectAccesses
            , mapper
            , unitOfWork
            , floors
            , buildings
            , floors
            , rooms
            , racks)
    {
    }

    protected override async Task<Guid[]> GetIDsSubObjectsAsync(Guid parentID, CancellationToken cancellationToken) 
        => await GetFloorsSubObjectsIDAsync(new Guid[] { parentID }, cancellationToken);

    protected override async Task InsertSubObjectsAsync(Guid parentID, Guid ownerID, CancellationToken cancellationToken)
        => await InsertRoomAccessAsync(parentID, ownerID, cancellationToken);
}
