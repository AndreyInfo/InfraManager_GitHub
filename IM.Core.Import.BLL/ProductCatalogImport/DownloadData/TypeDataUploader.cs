using System.Collections.Immutable;
using System.Reflection;
using IM.Core.Import.BLL.Interface.Import.Models;
using IM.Core.Import.BLL.Interface.Import.Models.DownloadData;
using IM.Core.Import.BLL.Interface.Import.Models.Import;
using IM.Core.Import.BLL.Interface.Import.Models.Settings;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using InfraManager.DAL.ProductCatalogue;

namespace IM.Core.Import.BLL.Import.Importer.DownloadData;

public class TypeDataUploader<TData> : ITypeDataUploader<TData>
where TData:IModelDataTryGet
{
    private readonly IReadonlyRepository<ProductCatalogType> _productCatalogTypes;
    private readonly ICommonPropertyData<ProductCatalogType, CommonData> _typeCommonPropertyData;

    public TypeDataUploader(IReadonlyRepository<ProductCatalogType> productCatalogTypes,
        ICommonPropertyData<ProductCatalogType, CommonData> typeCommonPropertyData)
    {
        _productCatalogTypes = productCatalogTypes;
        _typeCommonPropertyData = typeCommonPropertyData;
    }

    public async Task<ProductCatalogType?> GetTypeIDAsync(TData data, CancellationToken token)
    {
        var properties = _typeCommonPropertyData.GetProperties();
        
        var externalID = GetDataByPropertyName(data, properties, nameof(ProductCatalogType.ExternalID));
        if (externalID == null)
            return null;
        
        var externalName = GetDataByPropertyName(data, properties, nameof(ProductCatalogType.ExternalName));
        if (externalName == null)
            return null;
        
        
        var dataResult = _productCatalogTypes.With(x => x.ProductCatalogTemplate)
            .SingleOrDefault(x => x.ExternalID == externalID);

        return dataResult?.ExternalName?.Equals(externalName) ?? false ? dataResult : null;

    }

    public Task<bool> CheckExisting(TData data, CancellationToken token)
    {
        var properties = _typeCommonPropertyData.GetProperties();

        var externalID = GetDataByPropertyName(data, properties, nameof(ProductCatalogType.ExternalID));
        if (externalID == null)
            return Task.FromResult(false);

        var externalName = GetDataByPropertyName(data, properties, nameof(ProductCatalogType.ExternalName));
        if (externalName == null)
            return Task.FromResult(false);

        return Task.FromResult(true);

    }

    private static string? GetDataByPropertyName(TData data, IReadOnlyDictionary<string, PropertyInfo> properties, string propertyName)
    {
        var externalIDName = properties
            .Single(x => x.Value.Name == propertyName)
            .Key;
        if (data.TryGetValue(externalIDName, out var externalID))
            return externalID;
        return null;
    }
}