using AutoMapper;
using InfraManager.DAL.ProductCatalogue.LifeCycles;

namespace InfraManager.BLL.ProductCatalogue.LifeCycles.LifeCycleStates.LifeCycleStateOperations;

internal sealed class LifeCycleStateOperationProfile : Profile
{
    public LifeCycleStateOperationProfile()
    {
        CreateMap<LifeCycleStateOperation, LifeCycleStateOperationDetails>();

        CreateMap<LifeCycleStateOperationData, LifeCycleStateOperation>()
            .ConstructUsing(c=> new LifeCycleStateOperation(c.Name, c.Sequence, c.CommandType, c.LifeCycleStateID));

        CreateMap<LifeCycleStateOperationSubData, LifeCycleStateOperation>();
    }
}
