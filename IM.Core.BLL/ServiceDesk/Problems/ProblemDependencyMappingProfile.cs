using AutoMapper;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.Problems;

public class ProblemDependencyMappingProfile : Profile
{
    public ProblemDependencyMappingProfile()
    {
        CreateMap<ProblemDependencyDetailsModel, ProblemDependency>()
            .ForMember(dst => dst.OwnerObjectID,
                mapper => mapper.MapFrom(
                    src => src.ProblemID))
            .ForMember(dst => dst.ObjectClassID,
                mapper => mapper.MapFrom(
                    src => src.ClassID))
            .ForMember(dst => dst.Note,
                mapper => mapper.MapFrom(
                    _ => string.Empty));

        CreateMap<ProblemDependency, ProblemDependencyQueryResultItem>()
            .ForMember(dst => dst.ProblemID,
                mapper => mapper.MapFrom(
                    src => src.OwnerObjectID));
    }
}