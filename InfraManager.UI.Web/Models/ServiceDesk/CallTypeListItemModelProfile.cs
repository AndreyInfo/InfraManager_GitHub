using AutoMapper;
using InfraManager.Web.BLL.Helpers;
using BL = InfraManager.BLL.ServiceDesk.Calls;
using PL = InfraManager.UI.Web.Models.ServiceDesk;

namespace InfraManager.UI.Web.Models.ServiceDesk
{
    public class CallTypeListItemModelProfile : Profile
    {
        public CallTypeListItemModelProfile()
        {
            CreateMap<BL.CallTypeDetails, PL.CallTypeListItemModel>()
                .ForMember(entity => entity.ID, mapping => mapping.MapFrom(data => data.ID))
                .ForMember(entity => entity.Name, mapping => mapping.MapFrom(data => data.Name))
                .ForMember(entity => entity.FullName, mapping => mapping.MapFrom(data => data.FullName))
                .ForMember(entity => entity.IsRFC, mapping => mapping.MapFrom(data => data.IsRFC))
                .ForMember(entity => entity.IsIncident, mapping => mapping.MapFrom(data => data.IsIncident))
                .ForMember(entity => entity.ImageSource, mapping => mapping.MapFrom(data => data.HasNoImage ? null : ImageHelper.GetValidPath(ImageHelper.GetCallTypeIconSource(data.ID))))
                .ForMember(entity => entity.WorkflowSchemeIdentifier, mapping => mapping.MapFrom(data => data.WorkflowSchemeIdentifier));
                
        }
    }
}
