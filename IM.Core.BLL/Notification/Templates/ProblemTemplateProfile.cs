using System.Linq;
using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.Core;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.Notification.Templates;

public class ProblemTemplateProfile : Profile
{
    public ProblemTemplateProfile()
    {
        CreateMap<Problem, ProblemTemplate>()
            .ForMember(dst => dst.ID, opt => opt.MapFrom(src => src.IMObjID))
            .ForMember(dst => dst.NumberString,
                opt => opt.MapFrom(
                    src => src.ReferenceName))
            .ForMember(dst => dst.ProblemTypeFullName,
                opt => opt.MapFrom(
                    src => src.Type != null ? src.Type.FullName : string.Empty))
            .ForMember(dst => dst.OwnerLastName,
                opt => opt.MapFrom(
                    src => src.Owner != null ? src.Owner.Surname : string.Empty))
            .ForMember(dst => dst.OwnerFirstName,
                opt => opt.MapFrom(
                    src => src.Owner != null ? src.Owner.Name : string.Empty))
            .ForMember(dst => dst.OwnerLogin,
                opt => opt.MapFrom(
                    src => src.Owner != null ? src.Owner.LoginName : string.Empty))
            .ForMember(dst => dst.CallCountString,
                opt => opt.MapFrom(
                    src => src.CallReferences.Count().ToString()))
            .ForMember(dst => dst.WorkOrderCountString,
                opt => opt.MapFrom(
                    src => src.WorkOrderReferences.Count().ToString()))
            .ForMember(dst => dst.DependencyObjectCountString,
                opt => opt.MapFrom(
                    src => src.Dependencies.Count().ToString()))
            .ForMember(dst => dst.ManhoursString,
                opt => opt.MapFrom<ManhoursResolver<Problem, ProblemTemplate>, int>(
                    src => src.ManhoursInMinutes))
            .ForMember(dst => dst.ManhoursNormString,
                opt => opt.MapFrom<ManhoursResolver<Problem, ProblemTemplate>, int>(
                    src => src.ManhoursNormInMinutes))
            .ForMember(x => x.InitiatorFullName, x => x.NullSubstitute(string.Empty))
            .ForMember(x => x.InitiatorFirstName, x => x.MapFrom(x => x.Initiator.Name ?? string.Empty))
            .ForMember(x => x.InitiatorLastName, x => x.MapFrom(x => x.Initiator.Surname ?? string.Empty))
            .ForMember(x => x.ExecutorFullName, x => x.NullSubstitute(string.Empty))
            .ForMember(x => x.ExecutorFirstName, x => x.MapFrom(x => x.Executor.Name ?? string.Empty))
            .ForMember(x => x.ExecutorLastName, x => x.MapFrom(x => x.Executor.Surname ?? string.Empty))
            .ForMember(x => x.MainService, x => x.MapFrom(x => x.Service.FullLocation))
            .ForMember(dst => dst.NumberOnly, opt => opt.MapFrom(src => src.Number.ToString()))
            ;
    }
}