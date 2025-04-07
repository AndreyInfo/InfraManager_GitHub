using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.Location;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.AccessManagement.ResponsibilityTreeAccess.TOZ_sks;
internal sealed class SaveAccessRoomTOZ : SaveAccessAbstractTOZ<Room>
{
    public SaveAccessRoomTOZ(IRepository<ObjectAccess> objectAccesses
        , IMapper mapper
        , IUnitOfWork unitOfWork
        , IReadonlyRepository<Building> buildings
        , IReadonlyRepository<Floor> floors
        , IReadonlyRepository<Room> rooms) 
        : base(objectAccesses
            , mapper
            , unitOfWork
            , buildings
            , floors
            , rooms)
    {
    }

    protected override ObjectClass ClassID => ObjectClass.Room;

    protected override IReadOnlyCollection<ObjectClass> ChildClasses => Array.Empty<ObjectClass>();

    protected override Task<Guid[]> GetIDsSubObjectsAsync(Guid parentID, CancellationToken cancellationToken)
        => Task.FromResult(Array.Empty<Guid>());
    

    protected override async Task InsertSubObjectsAsync(Guid parentID, Guid ownerID, CancellationToken cancellationToken)
    {
    }
}
