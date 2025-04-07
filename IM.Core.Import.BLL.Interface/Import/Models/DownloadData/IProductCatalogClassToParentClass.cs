using InfraManager;

namespace IM.Core.Import.BLL.Interface.Import.Models.DownloadData;

public interface IProductCatalogClassToParentClass
{
    Task<ObjectClass?> GetBaseObjectClassAsync(ObjectClass objectClass, CancellationToken token);
}