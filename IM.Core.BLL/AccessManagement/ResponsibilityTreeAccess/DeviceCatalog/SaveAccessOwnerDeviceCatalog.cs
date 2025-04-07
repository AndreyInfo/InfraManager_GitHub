using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ProductCatalogue;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.AccessManagement.ResponsibilityTreeAccess.DeviceCatalog;
internal sealed class SaveAccessOwnerDeviceCatalog : SaveAccessAbstract<Owner>
{
    private readonly IReadonlyRepository<ProductCatalogCategory> _productCatalogCategories;
    private readonly IReadonlyRepository<ProductCatalogType> _productCatalogTypes;

    protected override AccessTypes AccessType => AccessTypes.DeviceCatalogue;
    protected override ObjectClass ClassID => ObjectClass.ProductCatalogue;

    public SaveAccessOwnerDeviceCatalog(IRepository<ObjectAccess> objectAccesses
        , IMapper mapper
        , IUnitOfWork unitOfWork
        , IReadonlyRepository<ProductCatalogCategory> productCatalogCategories
        , IReadonlyRepository<ProductCatalogType> productCatalogTypes)
        : base(objectAccesses
            , mapper
            , unitOfWork)
    {
        _productCatalogCategories = productCatalogCategories;
        _productCatalogTypes = productCatalogTypes;
    }

    protected override IReadOnlyCollection<ObjectClass> ChildClasses => new ObjectClass[]
    {
        ObjectClass.ProductCatalogCategory,
        ObjectClass.ProductCatalogType,
    };

    protected override Task<Guid[]> GetIDsSubObjectsAsync(Guid parentID, CancellationToken cancellationToken)
        => Task.FromResult(Array.Empty<Guid>());

    protected override async Task InsertSubObjectsAsync(Guid parentID, Guid ownerID, CancellationToken cancellationToken)
    {
        await InsertAllCategoryAsync(ownerID, cancellationToken);
        await InsertAllTypeAsync(ownerID, cancellationToken);
    }

    private async Task InsertAllCategoryAsync(Guid ownerID, CancellationToken cancellationToken)
    {
        var categories = await _productCatalogCategories.ToArrayAsync(cancellationToken);
        foreach (var category in categories)
            await InsertItemAsync(ownerID, category.ID, ObjectClass.ProductCatalogCategory, cancellationToken: cancellationToken);
    }

    private async Task InsertAllTypeAsync(Guid ownerID, CancellationToken cancellationToken)
    {
        var categories = await _productCatalogTypes.ToArrayAsync(cancellationToken);
        foreach (var category in categories)
            await InsertItemAsync(ownerID, category.IMObjID, ObjectClass.ProductCatalogType, cancellationToken: cancellationToken);
    }
}
