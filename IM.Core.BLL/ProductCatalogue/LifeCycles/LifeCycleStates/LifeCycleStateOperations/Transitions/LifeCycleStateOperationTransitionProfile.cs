using AutoMapper;
using InfraManager.DAL.ProductCatalogue.LifeCycles;

namespace InfraManager.BLL.ProductCatalogue.LifeCycles.LifeCycleStates.LifeCycleStateOperations.Transitions;

internal sealed class LifeCycleStateOperationTransitionProfile : Profile
{
    public LifeCycleStateOperationTransitionProfile()
    {
        CreateMap<LifeCycleStateOperationTransition, LifeCycleStateOperationTransitionDetails>()
            .ForMember(dst => dst.FinishStateName, m => m.MapFrom(scr => scr.FinishState.Name));

        CreateMap<LifeCycleStateOperationTransitionData, LifeCycleStateOperationTransition>()
            .ConstructUsing(c => new LifeCycleStateOperationTransition(c.OperationID, c.FinishStateID, c.Mode));

        CreateMap<LifeCycleStateOperationTransitionSubData, LifeCycleStateOperationTransition>();
    }
}
