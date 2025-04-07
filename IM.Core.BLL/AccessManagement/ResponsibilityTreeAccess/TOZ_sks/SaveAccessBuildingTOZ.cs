using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.Location;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.AccessManagement.ResponsibilityTreeAccess.TOZ_sks;
internal sealed class SaveAccessBuildingTOZ : SaveAccessAbstractTOZ<Building>
{
    protected override ObjectClass ClassID => ObjectClass.Building;
    protected override IReadOnlyCollection<ObjectClass> ChildClasses => new ObjectClass[]
    {
        ObjectClass.Floor,
        ObjectClass.Room,
    };

    public SaveAccessBuildingTOZ(IRepository<ObjectAccess> objectAccesses
        , IMapper mapper
        , IUnitOfWork unitOfWork
        , IReadonlyRepository<Building> buildings
        , IReadonlyRepository<Floor> floors
        , IReadonlyRepository<Room> rooms
        ) 
        : base(objectAccesses
            , mapper
            , unitOfWork
            , buildings
            , floors
            , rooms)
    {
    }


    protected override async Task<Guid[]> GetIDsSubObjectsAsync(Guid parentID, CancellationToken cancellationToken)
        => await GetBuildingSubObjectsIDAsync(new Guid[] { parentID }, cancellationToken);

    protected override async Task InsertSubObjectsAsync(Guid parentID, Guid ownerID, CancellationToken cancellationToken) 
        => await InsertFloorAccessAsync(parentID, ownerID, cancellationToken);

}
