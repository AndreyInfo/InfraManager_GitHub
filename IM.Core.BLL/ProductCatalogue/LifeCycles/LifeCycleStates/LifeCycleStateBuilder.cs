using AutoMapper;
using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.LifeCycles.LifeCycleStates;
internal sealed class LifeCycleStateBuilder :
    IBuildObject<LifeCycleState, LifeCycleStateData>
    , ISelfRegisteredService<IBuildObject<LifeCycleState, LifeCycleStateData>>
{
    private readonly IMapper _mapper;
    private readonly IReadonlyRepository<LifeCycleState> _repository;

    public LifeCycleStateBuilder(IMapper mapper
        , IReadonlyRepository<LifeCycleState> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }
    public async Task<LifeCycleState> BuildAsync(LifeCycleStateData data, CancellationToken cancellationToken = default)
    {
        if (data.IsDefault)
        {
            var states = await _repository.ToArrayAsync(c=> c.LifeCycleID == data.LifeCycleID, cancellationToken);
            states.ForEach(c=> c.IsDefault = false);
        }

        return _mapper.Map<LifeCycleState>(data);
    }

    public Task<IEnumerable<LifeCycleState>> BuildManyAsync(IEnumerable<LifeCycleStateData> dataItems, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
