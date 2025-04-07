using AutoMapper;
using IM.Core.Import.BLL.Interface.Import;
using InfraManager.DAL;

namespace IM.Core.Import.BLL.Import;

public class CommonUpdateQuery<TModel,TEntity>:IUpdateQuery<TModel,TEntity>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CommonUpdateQuery(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, TModel model, CancellationToken token)
    {
        _mapper.Map(model, entity);
        await _unitOfWork.SaveAsync(token);

        return entity;
    }
}