using AutoMapper;
using InfraManager.DAL.ProductCatalogue.LifeCycles;

namespace InfraManager.BLL.ProductCatalogue.LifeCycles;

internal sealed class LifeCycleProfile : Profile
{
    public LifeCycleProfile()
    {
        CreateMap<LifeCycle, LifeCycleDetails>()
            .ForMember(c => c.Fixed, m => m.MapFrom(scr => scr.IsFixed))
            .ForMember(c => c.States, m => m.MapFrom(scr => scr.LifeCycleStates));

        CreateMap<LifeCycleData, LifeCycle>()
            .ConstructUsing(c => new LifeCycle(c.Name, c.Type))
            .ForMember(c => c.LifeCycleStates, m => m.MapFrom(scr => scr.States));
    }
}
