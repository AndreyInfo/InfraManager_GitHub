using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL;

namespace InfraManager.BLL.ProductCatalogue.Import;

public static class ImportExtensions
{
    public static async Task<TEntity> FindOrRaiseErrorAsync<TEntity>(
        this IFinder<TEntity> finder,
        ProductCatalogImportCSVConcordanceKey key,
        CancellationToken cancellationToken = default)
    {

        return await finder.FindAsync(key, cancellationToken)
               ?? throw new ObjectNotFoundException(key.Field);
    }

}