using System;
using System.Linq;
using Inframanager.BLL.EventsOld;
using InfraManager.DAL;
using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogCategories;

public class ProductCatalogCategoryEventBuilder:
    IConfigureEventBuilder<ProductCatalogCategory>
{
    private readonly IReadonlyRepository<ProductCatalogCategory> _categories;

    public ProductCatalogCategoryEventBuilder(IReadonlyRepository<ProductCatalogCategory> categories)
    {
        _categories = categories;
    }

    public void Configure(IBuildEvent<ProductCatalogCategory> config)
    {
        config.HasEntityName(nameof(ProductCatalogCategory));
        config.HasId(x => x.ID);
        config.HasInstanceName(x => x.Name);
        config.HasProperty(x => x.Name).HasName("Название");
        config.HasProperty(x => x.Icon).HasConverter(Convert.ToHexString).HasName("Иконка");
        config.HasProperty(x => x.ParentProductCatalogCategoryID)
            .HasConverter(x=>x.HasValue?$"{_categories.Query().Select(y=>DbFunctions.GetCategoryFullName(x.Value)).FirstOrDefault()} (ID={x})":"null (ID=null)")
            .HasName("Родительская категория");
    }

    public void WhenInserted(IBuildEventOperation<ProductCatalogCategory> insertConfig)
    {
        insertConfig.HasOperation(OperationID.ProductCatalogCategory_Insert,
            x => $"Создана [Категория каталога продуктов] '{x.Name}'");
    }

    public void WhenUpdated(IBuildEventOperation<ProductCatalogCategory> updateConfig)
    {
        updateConfig.HasOperation(OperationID.ProductCatalogCategory_Update,
            x => $"Обновлена [Категория каталога продуктов] '{x.Name}'");
    }

    public void WhenDeleted(IBuildEventOperation<ProductCatalogCategory> deleteConfig)
    {
        deleteConfig.HasOperation(OperationID.ProductCatalogCategory_Delete,
            x => $"Удалена [Категория каталога продуктов] '{x.Name}'");
    }
}