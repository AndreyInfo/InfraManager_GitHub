using IM.Core.Import.BLL.Interface.Exceptions;
using IM.Core.Import.BLL.Interface.Import;
using InfraManager.DAL;

namespace IM.Core.Import.BLL.Import;

public class CommonRemoveQuery<TKey,TEntity>:IRemoveQuery<TKey,TEntity> where TEntity:class
{
    private readonly IFinderQuery<TKey, TEntity> _finder;
    private readonly IRepository<TEntity> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CommonRemoveQuery(IFinderQuery<TKey, TEntity> finder,
        IRepository<TEntity> repository,
        IUnitOfWork unitOfWork)
    {
        _finder = finder;
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task RemoveAsync(TKey key, CancellationToken token)
    {
        var entity = await _finder.GetFindQueryAsync(key, token);
        if (entity == null)
            throw new ObjectNotFoundException($"Не найден {typeof(TEntity).Name} с ID ={key}");
        _repository.Delete(entity);
    }
}