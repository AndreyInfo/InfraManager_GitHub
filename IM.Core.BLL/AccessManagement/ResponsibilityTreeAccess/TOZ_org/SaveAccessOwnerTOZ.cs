using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.OrganizationStructure;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.AccessManagement.ResponsibilityTreeAccess.TOZ_org;
internal sealed class SaveAccessOwnerOrgTOZ : SaveAccessAbstract<Owner>
{
    private readonly IReadonlyRepository<Organization> _organizations;
    private readonly IReadonlyRepository<Subdivision> _subdivisions;

    protected override AccessTypes AccessType => AccessTypes.TOZ_org;
    protected override ObjectClass ClassID => ObjectClass.Owner;
    protected override IReadOnlyCollection<ObjectClass> ChildClasses => new ObjectClass[]
    {
        ObjectClass.Organizaton,
        ObjectClass.Division,
    };

    public SaveAccessOwnerOrgTOZ(IRepository<ObjectAccess> objectAccesses
        , IMapper mapper
        , IUnitOfWork unitOfWork
        , IReadonlyRepository<Organization> organizations
        , IReadonlyRepository<Subdivision> subdivisions)
        : base(objectAccesses
            , mapper
            , unitOfWork)
    {
        _organizations = organizations;
        _subdivisions = subdivisions;
    }

    protected override Task<Guid[]> GetIDsSubObjectsAsync(Guid parentID, CancellationToken cancellationToken)
        => Task.FromResult(Array.Empty<Guid>());

    protected override async Task InsertSubObjectsAsync(Guid parentID, Guid ownerID, CancellationToken cancellationToken)
    {
        await InsertAllOrganizationAccessAsync(ownerID, cancellationToken);
        await InsertAllDivisionAccessAsync(ownerID, cancellationToken);
    }

    private async Task InsertAllOrganizationAccessAsync(Guid ownerID, CancellationToken cancellationToken)
    {
        var orgnaizations = await _organizations.ToArrayAsync(cancellationToken);
        foreach (var organization in orgnaizations)
            await InsertItemAsync(ownerID, organization.ID, ObjectClass.Organizaton, cancellationToken: cancellationToken);
    }

    private async Task InsertAllDivisionAccessAsync(Guid ownerID, CancellationToken cancellationToken)
    {
        var divisions = await _subdivisions.ToArrayAsync(cancellationToken);
        foreach (var division in divisions)
            await InsertItemAsync(ownerID, division.ID, ObjectClass.Division, cancellationToken: cancellationToken);
    }
}
