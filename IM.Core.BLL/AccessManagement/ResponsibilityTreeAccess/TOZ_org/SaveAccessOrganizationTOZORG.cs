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
internal sealed class SaveAccessOrganizationTOZORG : SaveAccessOrgTOZAbstract<Organization>
{
    protected override ObjectClass ClassID => ObjectClass.Organizaton;
    protected override IReadOnlyCollection<ObjectClass> ChildClasses => new ObjectClass[]
    {
        ObjectClass.Division,
    };

    public SaveAccessOrganizationTOZORG(IRepository<ObjectAccess> objectAccesses
        , IMapper mapper
        , IUnitOfWork unitOfWork
        , IReadonlyRepository<Subdivision> divisions)
        : base(objectAccesses
            , mapper
            , unitOfWork
            , divisions)
    {
    }

    protected override async Task InsertSubObjectsAsync(Guid parentID, Guid ownerID, CancellationToken cancellationToken) 
        => await InsertDivisionAsync(x => x.OrganizationID == parentID, ownerID, cancellationToken);

    protected override async Task<Guid[]> GetIDsSubObjectsAsync(Guid parentID, CancellationToken cancellationToken)
    {
        var divisions = await _subdivisionRepository.ToArrayAsync(c => c.OrganizationID == parentID, cancellationToken);
        var result = new List<Guid>();
        foreach (var division in divisions)
        {
            var subdivisions = await GetAllSubdivisionAsync(division.ID, cancellationToken);

            result.AddRange(subdivisions.Select(x => x.ID));
        }

        return result.ToArray();
    }
}
