using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.ProductCatalogue;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.AccessManagement.ResponsibilityTreeAccess.DeviceCatalog;
internal sealed class SaveAccessProductCatalogType : SaveAccessAbstract<ProductCatalogType>
{
    protected override AccessTypes AccessType => AccessTypes.DeviceCatalogue;
    protected override ObjectClass ClassID => ObjectClass.ProductCatalogType;
    protected override IReadOnlyCollection<ObjectClass> ChildClasses => Array.Empty<ObjectClass>();

    public SaveAccessProductCatalogType(IRepository<ObjectAccess> objectAccesses
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
        => Task.FromResult(Array.Empty<Guid>());
}
