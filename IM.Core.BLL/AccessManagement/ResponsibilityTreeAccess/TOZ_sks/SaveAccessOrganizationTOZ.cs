using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.Location;
using InfraManager.DAL.OrganizationStructure;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.AccessManagement.ResponsibilityTreeAccess.TOZ_sks;
internal sealed class SaveAccessOrganizationTOZ : SaveAccessAbstractTOZ<Organization>
{
    protected override ObjectClass ClassID => ObjectClass.Organizaton;
    protected override IReadOnlyCollection<ObjectClass> ChildClasses => new HashSet<ObjectClass> 
    { 
        ObjectClass.Building,
        ObjectClass.Floor,
        ObjectClass.Room,
    };

    public SaveAccessOrganizationTOZ(IRepository<ObjectAccess> objectAccesses
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

    protected override async Task<Guid[]> GetIDsSubObjectsAsync(Guid parentID, CancellationToken cancellationToken)
        => await GetOrganizationSubObjectsIDAsync(parentID, cancellationToken);

    protected override async Task InsertSubObjectsAsync(Guid parentID, Guid ownerID, CancellationToken cancellationToken)
        => await InsertBuildingAccessAsync(parentID, ownerID, cancellationToken);
}
