using AutoMapper;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Models;
using InfraManager.BLL;
using InfraManager.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace IM.Core.Import.BLL.Import;

public abstract class BaseEntityBLL<TKey, TEntity, TIntput, TOutput,TFilter> : 
    IBaseBLL<TKey, TEntity, TIntput, TOutput,TFilter> where TEntity:class
{
    private readonly IRepository<TEntity> _entities;
    private readonly IMapper _mapper;
    private readonly IFilterEntity<TEntity, TFilter> _filterEntity;
    private readonly IFinderQuery<TKey,TEntity> _finder;
    private readonly IBuildModel<TEntity, TOutput> _outputBuilder;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUpdateQuery<TIntput, TEntity> _updateQuery;
    private readonly IInsertQuery<TIntput, TEntity> _insertQuery;
    private readonly IRemoveQuery<TKey, TEntity> _removeQuery;


    public BaseEntityBLL(IRepository<TEntity> entities,
        IMapper mapper,
        IFilterEntity<TEntity, TFilter> filterEntity,
        IFinderQuery<TKey,TEntity> finder,
        IUnitOfWork unitOfWork, 
        IBuildModel<TEntity, TOutput> outputBuilder, 
        IUpdateQuery<TIntput, TEntity> updateQuery,
        IInsertQuery<TIntput, TEntity> insertQuery, 
        IRemoveQuery<TKey, TEntity> removeQuery)
    {
        _entities = entities;
        _mapper = mapper;
        _filterEntity = filterEntity;
        _finder = finder;
        _unitOfWork = unitOfWork;
        _outputBuilder = outputBuilder;
        _updateQuery = updateQuery;
        _insertQuery = insertQuery;
        _removeQuery = removeQuery;
    }

    public async Task<TOutput[]> GetDetailsArrayAsync(TFilter filter, CancellationToken cancellationToken)
    {
        var resultQuery = _filterEntity.Query(filter);
        var entities = await resultQuery.ToArrayAsync(cancellationToken);
        var output = await _outputBuilder.BuildArrayAsync(entities, cancellationToken);

        return output;
    }

    public async Task<TOutput> DetailsAsync(TKey id, CancellationToken cancellationToken)
    {
        var entity = await _finder.GetFindQueryAsync(id, cancellationToken);
        var output = await _outputBuilder.BuildAsync(entity, cancellationToken);
        return output;
    }


    public async Task<TOutput> AddAsync(TIntput data, CancellationToken cancellationToken = default)
    {
        var entity = await _insertQuery.AddAsync(data, cancellationToken);
        var afterSave =  await _outputBuilder.BuildAsync(entity, cancellationToken);
        return afterSave;
    }

    public async Task<TOutput> UpdateAsync(TKey id, TIntput data, CancellationToken cancellationToken = default)
    {
        var entity = await _finder.GetFindQueryAsync(id, cancellationToken);
        if (entity == null)
            throw new ObjectNotFoundException($"Не найден {typeof(TEntity).Name} с ID = {id}");

        entity = await _updateQuery.UpdateAsync(entity, data, cancellationToken);
        
        var output =  await _outputBuilder.BuildAsync(entity, cancellationToken);
        return output;
    }

    public async Task DeleteAsync(TKey id, CancellationToken cancellationToken = default)
    { 
        await _removeQuery.RemoveAsync(id, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}