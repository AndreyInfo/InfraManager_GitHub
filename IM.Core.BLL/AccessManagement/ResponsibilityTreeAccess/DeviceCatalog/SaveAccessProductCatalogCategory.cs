using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.ProductCatalogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.AccessManagement.ResponsibilityTreeAccess.DeviceCatalog;
internal class SaveAccessProductCatalogCategory : SaveAccessAbstract<ProductCatalogCategory>
{
    protected override AccessTypes AccessType => AccessTypes.DeviceCatalogue;
    protected override ObjectClass ClassID => ObjectClass.ProductCatalogCategory;
    protected override IReadOnlyCollection<ObjectClass> ChildClasses => new ObjectClass[]
    {
        ObjectClass.ProductCatalogCategory,
        ObjectClass.ProductCatalogType
    };
    private readonly IReadonlyRepository<ProductCatalogCategory> _productCatalogCategories;
    public SaveAccessProductCatalogCategory(IRepository<ObjectAccess> objectAccesses
    , IMapper mapper
    , IUnitOfWork unitOfWork
    , IReadonlyRepository<ProductCatalogCategory> productCatalogCategories)
    : base(objectAccesses
        , mapper
        , unitOfWork)
    {
        _productCatalogCategories = productCatalogCategories;
    }

    protected override async Task InsertSubObjectsAsync(Guid parentID, Guid ownerID, CancellationToken cancellationToken)
    {
        var categories = await GetAllSubCategoriesByIDAsync(parentID, cancellationToken);
        foreach (var category in categories)
        {
            await InsertTypeAccessAsync(category, ownerID, cancellationToken);
            if(category.ID != parentID)
                await InsertItemAsync(ownerID, category.ID, ObjectClass.ProductCatalogCategory, cancellationToken: cancellationToken);
        }
    }

    private async Task InsertTypeAccessAsync(ProductCatalogCategory category, Guid ownerID, CancellationToken cancellationToken)
    {
        foreach (var type in category.ProductCatalogTypes)
            await InsertItemAsync(ownerID, type.IMObjID, ObjectClass.ProductCatalogType, cancellationToken: cancellationToken);
    }


    protected override async Task<Guid[]> GetIDsSubObjectsAsync(Guid parentID, CancellationToken cancellationToken)
    {
        var categories = await GetAllSubCategoriesByIDAsync(parentID, cancellationToken);

        var result = new List<Guid>(categories.Select(c=> c.ID));

        var typesID = categories.SelectMany(c => c.ProductCatalogTypes.Select(v => v.IMObjID));
        result.AddRange(typesID);

        return result.ToArray();
    }

    protected async Task<ProductCatalogCategory[]> GetAllSubCategoriesByIDAsync(Guid id, CancellationToken cancellationToken)
    {
        var root = await _productCatalogCategories.WithMany(c => c.SubCategories)
                                  .WithMany(c=> c.ProductCatalogTypes)
                   .FirstOrDefaultAsync(x => x.ID == id, cancellationToken)
                   ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.ProductCatalogCategory);

        var result = new Queue<ProductCatalogCategory>();
        var nodes = new Queue<ProductCatalogCategory>();
        do
        {
            result.Enqueue(root);

            foreach (var item in root.SubCategories)
                nodes.Enqueue(item);

        } while (nodes.TryDequeue(out root));

        return result.ToArray();
    }
}
