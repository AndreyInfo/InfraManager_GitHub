using AutoMapper;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.Problems;

public class ProblemTypeProfile : Profile
{
    public ProblemTypeProfile()
    {
        CreateMap<ProblemType, ProblemTypeDetails>()
            .ForMember(dst => dst.WorkflowSchemeIdentifier, m => m.MapFrom(scr => scr.WorkflowSchemeIdentifier))
            .ForMember(dst => dst.ParentID, m => m.MapFrom(scr => scr.ParentProblemTypeID))
            .ReverseMap()
            .ForMember(dst => dst.ParentProblemTypeID, m => m.MapFrom(scr => scr.ParentID))
            .ForMember(dst => dst.WorkflowScheme, m => m.Ignore())
            .ForMember(dst => dst.Parent, m => m.Ignore());

        CreateMap<ProblemTypeData, ProblemType>()
            .ForMember(dst => dst.ParentProblemTypeID, m => m.MapFrom(scr => scr.ParentID));
    }
}
