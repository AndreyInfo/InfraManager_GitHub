using IM.Core.Import.BLL.Interface.Import.Models.DownloadData;
using IM.Core.Import.BLL.Interface.Import.Models.FieldUpdater;
using InfraManager.DAL;

namespace IM.Core.Import.BLL.Import.Importer.UploadData;

public class ModelTypeIDUpdater<TData,TEntity>:IModelTypeIDUpdater<TData,TEntity>
where TEntity:IImportPostLinkParameters
{
    private readonly ITypeDataUploader<TData> _typeDataUploader;

    public ModelTypeIDUpdater(ITypeDataUploader<TData> typeDataUploader)
    {
        _typeDataUploader = typeDataUploader;
    }

    public async Task<bool> SetFieldsAsync(TData data, TEntity entity,
        CancellationToken token)
    {
        var type = await _typeDataUploader.GetTypeIDAsync(data, token);
        if (type == null)
            return false;
        entity.ProductCatalogTypeID = type.IMObjID;
        return true;
    }
}