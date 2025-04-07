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
internal sealed class SaveAccessOrganizationTTZ : SaveAccessAbstractTTZ<Organization>
{
    protected override ObjectClass ClassID => ObjectClass.Organizaton;
    protected override IReadOnlyCollection<ObjectClass> ChildClasses => new ObjectClass[]
    {
        ObjectClass.Building,
        ObjectClass.Floor,
        ObjectClass.Room,
        ObjectClass.Rack,
    };


    public SaveAccessOrganizationTTZ(IRepository<ObjectAccess> objectAccesses
        , IMapper mapper
        , IUnitOfWork unitOfWork
        , IReadonlyRepository<Organization> organizations
        , IReadonlyRepository<Building> buildings
        , IReadonlyRepository<Floor> floors
        , IReadonlyRepository<Room> rooms
        , IReadonlyRepository<Rack> racks)
        : base(objectAccesses
            , mapper
            , unitOfWork
            , organizations
            , buildings
            , floors
            , rooms
            , racks)
    {
    }

    protected override async Task<Guid[]> GetIDsSubObjectsAsync(Guid parentID, CancellationToken cancellationToken)
        => await GetOrganizationSubObjectsIDAsync(parentID, cancellationToken);

    protected override async Task InsertSubObjectsAsync(Guid parentID, Guid ownerID, CancellationToken cancellationToken)
        => await InsertBuildingAccessAsync(parentID, ownerID, cancellationToken);
}
