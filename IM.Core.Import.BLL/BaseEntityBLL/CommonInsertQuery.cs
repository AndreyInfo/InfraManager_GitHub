using AutoMapper;
using IM.Core.Import.BLL.Interface.Import;
using InfraManager.DAL;

namespace IM.Core.Import.BLL.Import;

public class CommonInsertQuery<TModel, TEntity>:IInsertQuery<TModel, TEntity> where TEntity: class
{
    private readonly IMapper _mapper;
    private readonly IRepository<TEntity> _entities;
    private readonly IUnitOfWork _unitOfWork;
    public CommonInsertQuery(IMapper mapper, IRepository<TEntity> entities, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _entities = entities;
        _unitOfWork = unitOfWork;
    }

    public async Task<TEntity> AddAsync(TModel data, CancellationToken token)
    {
        var entity = _mapper.Map<TEntity>(data);
        
        _entities.Insert(entity);
       
        await _unitOfWork.SaveAsync(token);

        return entity;
    }
}