using IM.Core.Import.BLL.Interface.Import.Models.DownloadData;
using IM.Core.Import.BLL.Interface.Import.Models.UploadData;
using InfraManager;

namespace IM.Core.Import.BLL.Import.Importer.UploadData;

public class ModelSaverFacade<TData>:IModelSaver<TData>
{
    private readonly IServiceMapper<ObjectClass, ISaver<TData>> _modelEntitySaver;
    private readonly ITypeDataUploader<TData> _typeDataUploader;
    private readonly IProductCatalogClassToParentClass _catalogClass;
    private readonly HashSet<ObjectClass> _objects = new();
    public ModelSaverFacade(
         IServiceMapper<ObjectClass, ISaver<TData>> modelEntitySaver,
        ITypeDataUploader<TData> typeDataUploader,
        IProductCatalogClassToParentClass catalogClass
        )
    {
        _modelEntitySaver = modelEntitySaver;
        _typeDataUploader = typeDataUploader;
        _catalogClass = catalogClass;
    }

    private async Task<ISaver<TData>?> GetSaverAsync(TData data, CancellationToken token)
    {
        var productCatalogType = await _typeDataUploader.GetTypeIDAsync(data, token);
        if (productCatalogType == null)
            return null;

        var classID = productCatalogType.ProductCatalogTemplate.ClassID;   
        
        var realClass = await _catalogClass.GetBaseObjectClassAsync(classID, token);
        if (!realClass.HasValue)
            return null;

        if (_modelEntitySaver.HasKey(realClass.Value))
        {
            _objects.Add(realClass.Value);
            return _modelEntitySaver.Map(realClass.Value);
        }
       
        return null;
    }

    public async Task<bool> AddToBatchDataAsync(TData data, CancellationToken token)
    {
        var bll = await GetSaverAsync(data, token);
        if (bll == null)
            return false;

        return await bll.AddToBatchDataAsync(data, token);
    }

    public async Task SaveBatchAsync(CancellationToken token)
    {
        foreach (var objectClass in _objects.ToList())
        {
            if (!_modelEntitySaver.HasKey(objectClass))
                continue;
            
            var bll = _modelEntitySaver.Map(objectClass);
            
            if (bll == null)
                continue;

            await bll.SaveBatchAsync(token);
        }
       
    }

    public async Task DeleteAsync(CancellationToken token)
    {
        foreach (var objectClass in _objects.ToList())
        {
            if (!_modelEntitySaver.HasKey(objectClass))
                continue;
            
            var bll = _modelEntitySaver.Map(objectClass);
            
            if (bll == null)
                continue;

            await bll.DeleteAsync(token);
        }
    }
}