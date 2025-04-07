using System.Linq;
using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.DAL.ServiceDesk.ChangeRequests;

namespace InfraManager.BLL.Notification.Templates;

public class ChangeRequestTemplateProfile : Profile
{
    public ChangeRequestTemplateProfile()
    {
        CreateMap<ChangeRequest, RFCTemplate>()
            .ForMember(dst => dst.ID, opt => opt.MapFrom(src => src.IMObjID))
            .ForMember(dst => dst.NumberString,
                opt => opt.MapFrom(
                    src => src.ReferenceName))
            .ForMember(dst => dst.RFCTypeFullName,
                opt => opt.MapFrom(
                    src => src.Type != null ? src.Type.Name : string.Empty))
            .ForMember(dst => dst.RFCCategoryFullName,
                opt => opt.MapFrom(
                    src => src.Category != null ? src.Category.Name : string.Empty))
            .ForMember(dst => dst.ManhoursString,
                opt => opt.MapFrom<ManhoursResolver<ChangeRequest, RFCTemplate>, int>(
                    src => src.ManhoursInMinutes))
            .ForMember(dst => dst.ManhoursNormString,
                opt => opt.MapFrom<ManhoursResolver<ChangeRequest, RFCTemplate>, int>(
                    src => src.ManhoursNormInMinutes))
            .ForMember(dst => dst.InitiatorLastName,
                opt => opt.MapFrom(
                    src => src.Initiator != null ? src.Initiator.Surname : string.Empty))
            .ForMember(dst => dst.InitiatorFirstName,
                opt => opt.MapFrom(
                    src => src.Initiator != null ? src.Initiator.Name : string.Empty))
            .ForMember(dst => dst.InitiatorLogin,
                opt => opt.MapFrom(
                    src => src.Initiator != null ? src.Initiator.LoginName : string.Empty))
            .ForMember(dst => dst.OwnerLastName,
                opt => opt.MapFrom(
                    src => src.Owner != null ? src.Owner.Surname : string.Empty))
            .ForMember(dst => dst.OwnerFirstName,
                opt => opt.MapFrom(
                    src => src.Owner != null ? src.Owner.Name : string.Empty))
            .ForMember(dst => dst.OwnerLogin,
                opt => opt.MapFrom(
                    src => src.Owner != null ? src.Owner.LoginName : string.Empty))
            .ForMember(dst => dst.QueueName,
                opt => opt.MapFrom(
                    src => src.Group != null ? src.Group.Name : string.Empty))
            .ForMember(dst => dst.NumberOnly, opt => opt.MapFrom(src => src.Number.ToString()))
            ;
    }
}