using System.Linq;
using Inframanager;
using Inframanager.BLL.EventsOld;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogModels;

public abstract class ProductCatalogModelEventBuilder<T>
                where T:class,IProductModel
{
    private OperationID _insertOperationId;
    private OperationID _updateOperationId;
    private OperationID _deleteOperationId;
    
    private readonly string _modelName;
    private readonly IFinder<Manufacturer> _manufacturers;
    private readonly IFinder<ProductCatalogType> _productCatalogType;

    public ProductCatalogModelEventBuilder(
        string modelName,
        IFinder<Manufacturer> manufacturers,
        IFinder<ProductCatalogType> productCatalogType)
    {
        _modelName = modelName;
        _manufacturers = manufacturers;
        _productCatalogType = productCatalogType;
        InitOperations();
    }

    private void InitOperations()
    {
        var type = typeof(T);
       
        var operationIdAttributes = type.GetCustomAttributes(typeof(OperationIdMappingAttribute), false)
            .Cast<OperationIdMappingAttribute>().ToList();
        
        _insertOperationId = operationIdAttributes
            .Single(x => x.Action == ObjectAction.Insert).Operation;
        
        _updateOperationId = operationIdAttributes
             .Single(x => x.Action == ObjectAction.Update).Operation;
        
        _deleteOperationId = operationIdAttributes
             .Single(x => x.Action == ObjectAction.Delete).Operation;
    }
    

    public void Configure(IBuildEvent<T> config)
    {
        config.HasEntityName(typeof(T).Name);
        
        config.HasId(x => x.IMObjID);
        
        config.HasInstanceName(x => x.Name);
        
        config.HasProperty(x => x.Name).HasName("Название");
        
        config.HasProperty(x => x.Note).HasName("Заметки");
        
        config.HasProperty(x => x.ExternalID).HasName("Внешний ИД");
        
        config.HasProperty(x => x.ProductCatalogTypeID).HasConverter(x=>$"{_productCatalogType.Find(x)?.Name} (ID={x})").HasName("Тип");
        
    }

    public void WhenInserted(IBuildEventOperation<T> insertConfig)
    {
        insertConfig.HasOperation(_insertOperationId,
            x => $"Создана [{_modelName}] '{x.Name}'");
    }

    public void WhenUpdated(IBuildEventOperation<T> updateConfig)
    {
        updateConfig.HasOperation(_updateOperationId,
            x => $"Обновлена [{_modelName}] '{x.Name}'");
    }

    public void WhenDeleted(IBuildEventOperation<T> deleteConfig)
    {
        deleteConfig.HasOperation(_deleteOperationId,
            x => $"Удалена [{_modelName}] '{x.Name}'");
    }
}