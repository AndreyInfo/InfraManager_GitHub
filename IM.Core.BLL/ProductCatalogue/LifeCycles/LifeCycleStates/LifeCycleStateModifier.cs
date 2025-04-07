using AutoMapper;
using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.LifeCycles.LifeCycleStates;
internal sealed class LifeCycleStateModifier :
        IModifyObject<LifeCycleState, LifeCycleStateData>,
        ISelfRegisteredService<IModifyObject<LifeCycleState, LifeCycleStateData>>
{
    private readonly IMapper _mapper;
    private readonly IReadonlyRepository<LifeCycleState> _lifeCycleStates;

    public LifeCycleStateModifier(IMapper mapper
        , IReadonlyRepository<LifeCycleState> lifeCycleStates)
    {
        _mapper = mapper;
        _lifeCycleStates = lifeCycleStates;
    }

    public async Task ModifyAsync(LifeCycleState entity, LifeCycleStateData data, CancellationToken cancellationToken = default)
    {
        if (data.IsDefault)
        {
            var states = await _lifeCycleStates.ToArrayAsync(c=> c.IsDefault && c.LifeCycleID == data.LifeCycleID, cancellationToken);
            states.ForEach(c=> c.IsDefault = false);
        }

        _mapper.Map(data, entity);
    }

    public void SetModifiedDate(LifeCycleState entity)
    {
    }
}
