using InfraManager.DAL;

namespace InfraManager.BLL.Catalog
{
    public interface ICatalogDeletionBehaviour<TCatalog>
        where TCatalog : class
    {
        void DeleteCatalog(TCatalog catalog, IRepository<TCatalog> repository);
    }
}
