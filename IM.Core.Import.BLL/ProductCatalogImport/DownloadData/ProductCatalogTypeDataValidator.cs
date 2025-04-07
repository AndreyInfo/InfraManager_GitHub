using IM.Core.Import.BLL.Interface.Import.Models.DownloadData;
using IM.Core.Import.BLL.Interface.Import.Models.Import;
using InfraManager.DAL.Asset;
using InfraManager.DAL.Import;
using InfraManager.DAL.ProductCatalogue;

namespace IM.Core.Import.BLL.Import.Importer.DownloadData;

public class ProductCatalogTypeDataValidator<TData>:IValidator<TData, ProductCatalogType>
   
{
    private readonly IGetValidTypesQuery _validTypesQuery;
    private readonly ITypeDataUploader<TData> _typeDataUploader;
    public ProductCatalogTypeDataValidator(IGetValidTypesQuery validTypesQuery, ITypeDataUploader<TData> typeDataUploader)
    {
        _validTypesQuery = validTypesQuery;
        _typeDataUploader = typeDataUploader;
    }

    public async Task<bool> ValidateAsync(Guid id, TData model, CancellationToken token)
    {
        var type = await _typeDataUploader.GetTypeIDAsync(model, token);
        
        if (type == null)
            return false;
        
        var validIds = await _validTypesQuery.ExecuteAsync(id, token);
        
        return validIds.Contains(type.IMObjID);
    }
}