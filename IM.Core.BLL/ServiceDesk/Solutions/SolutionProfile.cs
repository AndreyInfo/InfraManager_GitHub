using AutoMapper;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.Solutions;

internal sealed class SolutionProfile : Profile
{
    public SolutionProfile()
    {
        CreateMap<Solution, SolutionDetails>()
            .ForMember(dst => dst.Description, m => m.MapFrom(scr => scr.Name));

        CreateMap<SolutionData, Solution >()
            .ForMember(dst => dst.Description, m => m.MapFrom(scr => scr.Name));
    }
}
