using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.Asset;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.AccessManagement.ResponsibilityTreeAccess.TTZ_sks;
internal sealed class SaveAccessRackTTZ : SaveAccessAbstract<Rack>
{
    protected override AccessTypes AccessType => AccessTypes.TTZ_sks;

    protected override ObjectClass ClassID => ObjectClass.Rack;

    protected override IReadOnlyCollection<ObjectClass> ChildClasses => Array.Empty<ObjectClass>();

    public SaveAccessRackTTZ(IRepository<ObjectAccess> objectAccesses
        , IMapper mapper
        , IUnitOfWork unitOfWork)
        : base(objectAccesses
            , mapper
            , unitOfWork)
    {
    }

    protected override async Task InsertSubObjectsAsync(Guid parentID, Guid ownerID, CancellationToken cancellationToken)
    {
    }

    protected override Task<Guid[]> GetIDsSubObjectsAsync(Guid parentID, CancellationToken cancellationToken)
    {
        return Task.FromResult(Array.Empty<Guid>());
    }
}
