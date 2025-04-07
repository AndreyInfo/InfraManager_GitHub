using IM.Core.Import.BLL.Interface.Import;
using InfraManager.DAL;

namespace IM.Core.Import.BLL.Import;

public class FinderQuery<TKey,TEntity>:IFinderQuery<TKey,TEntity>
{
    private readonly IFinder<TEntity> _finder;

    public FinderQuery(IFinder<TEntity> finder)
    {
        _finder = finder;
    }


    public async Task<TEntity> GetFindQueryAsync(TKey key, CancellationToken token)
    {
        return await _finder.FindAsync(new object[] {key}, token);
    }
}