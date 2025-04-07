using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.BLL.ServiceDesk.Problems;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.Problems;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.Problems.Mapping;

internal class ProblemDependencyProfile : Profile
{
    public ProblemDependencyProfile()
    {
        CreateMap<ProblemDependencyQueryResultItem, ProblemDependencyListItem>()
            .ForMember(dst => dst.EntityID, mapper => mapper.MapFrom(src => src.ProblemID))
            .ForMember(dst => dst.ObjectID, mapper => mapper.MapFrom(src => src.ObjectID))
            .ForMember(dst => dst.Name, mapper => mapper.MapFrom(src => src.ObjectName))
            .ForMember(dst => dst.Location, mapper => mapper.MapFrom(src => src.ObjectLocation))
            .ForMember(dst => dst.ClassName, mapper => mapper.MapFrom<ObjectClassNameResolver<ProblemDependencyQueryResultItem, ProblemDependencyListItem>, ObjectClass>(src => src.ObjectClassID))
            .ForMember(dst => dst.ClassID, mapper => mapper.MapFrom(src => src.ObjectClassID));
    }
}