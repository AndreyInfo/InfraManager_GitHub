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
internal sealed class SaveAccessBuildingTTZ : SaveAccessAbstractTTZ<Building>
{
    protected override ObjectClass ClassID => ObjectClass.Building;
    protected override IReadOnlyCollection<ObjectClass> ChildClasses => new ObjectClass[]
    {
        ObjectClass.Floor,
        ObjectClass.Room,
        ObjectClass.Rack,
    };


    public SaveAccessBuildingTTZ(IRepository<ObjectAccess> objectAccesses
        , IMapper mapper
        , IUnitOfWork unitOfWork
        , IReadonlyRepository<Building> buildings
        , IReadonlyRepository<Floor> floors
        , IReadonlyRepository<Room> rooms
        , IReadonlyRepository<Rack> racks)
        : base(objectAccesses
            , mapper
            , unitOfWork
            , buildings
            , buildings
            , floors
            , rooms
            , racks)
    {
    }

    protected override async Task<Guid[]> GetIDsSubObjectsAsync(Guid parentID, CancellationToken cancellationToken) 
        => await GetBuildingsSubObjectsIDAsync(new Guid[] { parentID }, cancellationToken);

    protected override async Task InsertSubObjectsAsync(Guid parentID, Guid ownerID, CancellationToken cancellationToken)
        => await InsertFloorAccessAsync(parentID, ownerID, cancellationToken);
}
