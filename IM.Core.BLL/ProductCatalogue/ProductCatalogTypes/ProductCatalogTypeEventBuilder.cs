using System;
using System.Linq;
using Inframanager.BLL.EventsOld;
using InfraManager.DAL;
using InfraManager.DAL.ProductCatalogue;
using InfraManager.DAL.ProductCatalogue.LifeCycles;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogTypes;

public class ProductCatalogTypeEventBuilder:
    IConfigureEventBuilder<ProductCatalogType>
{
    private readonly IReadonlyRepository<ProductCatalogCategory> _catalogCategories;
    private readonly IFinder<ProductCatalogTemplate> _catalogTemplates;
    private readonly IFinder<LifeCycle> _lifeCycles;
    public ProductCatalogTypeEventBuilder(
        IReadonlyRepository<ProductCatalogCategory> catalogCategories,
        IFinder<ProductCatalogTemplate> catalogTemplates,
        IFinder<LifeCycle> lifeCycles)
    {
        _catalogCategories = catalogCategories;
        _catalogTemplates = catalogTemplates;
        _lifeCycles = lifeCycles;
    }

    private static string BoolString(bool value) => value ? "да" : "нет";

    private static string BoolString(bool? value) => value.HasValue ? BoolString(value.Value) : "не определено";

    private string GetCategoryName(Guid categoryID)
    {
        var pathData = _catalogCategories.Query().Where(x => x.ID == categoryID)
            .Select(category => new
            {
                ParentPath = DbFunctions.GetCategoryFullName(categoryID),
                CurrentName = category.Name
            })
            .FirstOrDefault();
        if (pathData?.CurrentName == null)
            return null;

        if (string.IsNullOrWhiteSpace(pathData.ParentPath))
            return pathData.CurrentName;

        return $"{pathData.ParentPath}\\{pathData.CurrentName}";

    }
    
    public void Configure(IBuildEvent<ProductCatalogType> config)
    {
        config.HasEntityName(nameof(ProductCatalogType));
        config.HasId(x => x.IMObjID);
        config.HasInstanceName(x => x.Name);
        config.HasProperty(x => x.Name).HasName("Название");
        config.HasProperty(x => x.CanBuy).HasName("Использовать для закупки").HasConverter(BoolString);
        config.HasProperty(x => x.ExternalID).HasName("Внешний код");
        config.HasProperty(x => x.ExternalName).HasName("Внешнее название");
        config.HasProperty(x => x.IsLogical).HasName("Логический").HasConverter(BoolString);
        config.HasProperty(x => x.IsAccountingAsset).HasName("Подлежит учету активов").HasConverter(BoolString);
        config.HasProperty(x => x.LifeCycleID)
            .HasConverter(lifeCycleID => $"{_lifeCycles.Find(lifeCycleID)} (ID={lifeCycleID})")
            .HasName("Жизненный цикл");
            
        //SELECT TOP 1 GetCategoryClassFullName(@param1) FROM ProductCatalogCategories ~ SELECT GetCategoryClassFullName(@param1)
        config.HasProperty(x => x.ProductCatalogCategoryID)
            .HasConverter(categoryID=>$"{GetCategoryName(categoryID)} (ID = {categoryID})")
            .HasName("Категория");
        config.HasProperty(x => x.ProductCatalogTemplateID)
            .HasConverter(productCatalogTemplateID=>$"{_catalogTemplates.Find(productCatalogTemplateID)?.Name} (ID={productCatalogTemplateID})")
            .HasName("Класс");
    }
    
    public void WhenInserted(IBuildEventOperation<ProductCatalogType> insertConfig)
    {
        insertConfig.HasOperation(OperationID.ProductCatalogType_Insert,
            x => $"Создан [Тип каталога продуктов] '{x.Name}'");
    }

    public void WhenUpdated(IBuildEventOperation<ProductCatalogType> updateConfig)
    {
        updateConfig.HasOperation(OperationID.ProductCatalogType_Update,
            x => $"Обновлен [Тип каталога продуктов] '{x.Name}'");
    }

    public void WhenDeleted(IBuildEventOperation<ProductCatalogType> deleteConfig)
    {
        deleteConfig.HasOperation(OperationID.ProductCatalogType_Delete,
            x => $"Удален [Тип каталога продуктов] '{x.Name}'");
    }
}