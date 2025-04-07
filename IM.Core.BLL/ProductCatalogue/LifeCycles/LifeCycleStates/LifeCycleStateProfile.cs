using AutoMapper;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using System.Linq;

namespace InfraManager.BLL.ProductCatalogue.LifeCycles.LifeCycleStates;

internal sealed class LifeCycleStateProfile : Profile
{
    public LifeCycleStateProfile()
    {
        CreateMap<LifeCycleState, LifeCycleStateDetails>()
            .ForMember(dst => dst.Operations, m => m.MapFrom(scr => scr.LifeCycleStateOperations))
            .ForMember(dst => dst.Commands, m => m.MapFrom(scr => scr.LifeCycleStateOperations.Select(c=> c.CommandType)))
            .ForMember(dst => dst.Options, m => m.MapFrom(scr => new LifeCycleStateOptions(scr.IsWrittenOff
                                                                                           , scr.IsApplied
                                                                                           , scr.IsInRepair
                                                                                           , scr.CanCreateAgreement
                                                                                           , scr.CanIncludeInPurchase
                                                                                           , scr.CanIncludeInActiveRequest
                                                                                           , scr.CanIncludeInInfrastructure)));

        CreateMap<LifeCycleStateData, LifeCycleState>()
            .ConstructUsing(c=> new LifeCycleState(c.Name, c.LifeCycleID))
            .ForMember(dst => dst.IsWrittenOff, m => m.MapFrom(scr => scr.Options.IsWrittenOff))
            .ForMember(dst => dst.IsApplied, m => m.MapFrom(scr => scr.Options.IsApplied))
            .ForMember(dst => dst.IsInRepair, m => m.MapFrom(scr => scr.Options.IsInRepair))
            .ForMember(dst => dst.CanIncludeInInfrastructure, m => m.MapFrom(scr => scr.Options.CanIncludeInInfrastructure))
            .ForMember(dst => dst.CanCreateAgreement, m => m.MapFrom(scr => scr.Options.CanCreateAgreement))
            .ForMember(dst => dst.CanIncludeInActiveRequest, m => m.MapFrom(scr => scr.Options.CanIncludeInActiveRequest))
            .ForMember(dst => dst.CanIncludeInPurchase, m => m.MapFrom(scr => scr.Options.CanIncludeInPurchase))
            .ForMember(dst => dst.LifeCycleStateOperations, m => m.MapFrom(scr => scr.Operations));

        CreateMap<LifeCycleStateSubData, LifeCycleState>()
           .ForMember(dst => dst.IsWrittenOff, m => m.MapFrom(scr => scr.Options.IsWrittenOff))
           .ForMember(dst => dst.IsApplied, m => m.MapFrom(scr => scr.Options.IsApplied))
           .ForMember(dst => dst.IsInRepair, m => m.MapFrom(scr => scr.Options.IsInRepair))
           .ForMember(dst => dst.CanIncludeInInfrastructure, m => m.MapFrom(scr => scr.Options.CanIncludeInInfrastructure))
           .ForMember(dst => dst.CanCreateAgreement, m => m.MapFrom(scr => scr.Options.CanCreateAgreement))
           .ForMember(dst => dst.CanIncludeInActiveRequest, m => m.MapFrom(scr => scr.Options.CanIncludeInActiveRequest))
           .ForMember(dst => dst.CanIncludeInPurchase, m => m.MapFrom(scr => scr.Options.CanIncludeInPurchase))
           .ForMember(dst => dst.LifeCycleStateOperations, m => m.MapFrom(scr => scr.Operations));
    }
}
