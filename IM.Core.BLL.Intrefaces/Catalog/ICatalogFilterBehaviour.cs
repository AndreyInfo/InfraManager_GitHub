using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System.Linq;

namespace InfraManager.BLL.Catalog
{
    public interface ICatalogFilterBehaviour<TCatalog>
        where TCatalog : class
    {
        IQueryable<TCatalog> GetCatalogFilterQuery(IQueryable<TCatalog> catalogs, BaseFilter filter);
    }
}
