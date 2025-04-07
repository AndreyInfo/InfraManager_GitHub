using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.OrganizationStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.AccessManagement.ResponsibilityTreeAccess.TOZ_org;
internal sealed class SaveAccessDivisionTOZ : SaveAccessOrgTOZAbstract<Subdivision>
{
    protected override AccessTypes AccessType => AccessTypes.TOZ_org;
    protected override ObjectClass ClassID => ObjectClass.Division;
    protected override IReadOnlyCollection<ObjectClass> ChildClasses => new ObjectClass[]
    {
        ObjectClass.Division
    };

    public SaveAccessDivisionTOZ(IRepository<ObjectAccess> objectAccesses
        , IMapper mapper
        , IUnitOfWork unitOfWork
        , IReadonlyRepository<Subdivision> entities)
        : base(objectAccesses
            , mapper
            , unitOfWork
            , entities)
    {
    }

    protected override async Task InsertSubObjectsAsync(Guid parentID, Guid ownerID, CancellationToken cancellationToken)
        => await InsertDivisionAsync(x => x.SubdivisionID == parentID, ownerID, cancellationToken);

    protected override async Task<Guid[]> GetIDsSubObjectsAsync(Guid parentID, CancellationToken cancellationToken)
    {
        var subdivisions = await GetAllSubdivisionAsync(parentID, cancellationToken);

        return subdivisions.Select(c => c.ID).ToArray();
    }
}
