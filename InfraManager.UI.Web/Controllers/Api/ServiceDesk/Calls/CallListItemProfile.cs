using AutoMapper;
using InfraManager.BLL.ServiceDesk;
using InfraManager.BLL.ServiceDesk.Calls;
using InfraManager.UI.Web.AutoMapper;
using InfraManager.Web.BLL.Helpers;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.Calls;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.Calls
{
    public class CallListItemProfile : Profile
    {
        public CallListItemProfile()
        {
            CreateMap<CallListItem, CallListItemModel>()
                .ForMember(
                    model => model.Uri,
                    mapper => mapper.MapFrom<
                            PathResolver<CallListItem, CallListItemModel>,
                            InframanagerObject?>(
                                item => new InframanagerObject(item.ID, item.ClassID)))
                .ForMember(model => model.PriorityColor, mapper => mapper.MapFrom<PriorityColorResolver>());

            CreateMap<CallFromMeListItem, CallsFromMeListItemModel>()
                .ForMember(model => model.PriorityColor, mapper => mapper.MapFrom<PriorityColorResolver>())
                .ForMember(
                    model => model.Uri,
                    mapper => mapper.MapFrom<
                        PathResolver<CallFromMeListItem, CallsFromMeListItemModel>,
                        InframanagerObject?>(
                            item => new InframanagerObject(item.ID, item.ClassID)))
                .ForMember(
                    x => x.ImageSource,
                    m => m.MapFrom(
                        x => ImageHelper.GetValidPath(ImageHelper.GetCallTypeIconSource(x.TypeID))));
            CreateMap<CallReferenceListItem, CallReferenceListItemModel>();
                
        }
    }
}
