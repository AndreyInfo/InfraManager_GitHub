using InfraManager.DAL.Asset;
using InfraManager.DAL.ProductCatalogue;

namespace IM.Core.Import.BLL.Interface.Import.Models.DownloadData;

public interface ITypeDataUploader<TData>
{
    Task<ProductCatalogType?> GetTypeIDAsync(TData data, CancellationToken token);
    Task<bool> CheckExisting(TData data, CancellationToken token);
}