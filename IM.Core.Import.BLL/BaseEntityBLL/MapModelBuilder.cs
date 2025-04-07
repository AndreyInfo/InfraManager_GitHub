using System.Text.RegularExpressions;
using AutoMapper;
using IM.Core.Import.BLL.Interface.Import;

namespace IM.Core.Import.BLL.Import;

public class MapModelBuilder<TEntity, TModel>:IBuildModel<TEntity, TModel>
{
    private readonly IMapper _mapper;

    public MapModelBuilder(IMapper mapper)
    {
        _mapper = mapper;
    }

    public Task<TModel> BuildAsync(TEntity entity, CancellationToken token)
    {
        return Task.FromResult(_mapper.Map<TModel>(entity));
    }

    public Task<TModel[]> BuildArrayAsync(TEntity[] entities, CancellationToken token)
    {
        return Task.FromResult(_mapper.Map<TModel[]>(entities));
    }
}