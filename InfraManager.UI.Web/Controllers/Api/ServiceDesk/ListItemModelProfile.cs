using AutoMapper;
using InfraManager.BLL.ServiceDesk;
using InfraManager.BLL.ServiceDesk.CustomControl;
using InfraManager.UI.Web.AutoMapper;
using InfraManager.WebApi.Contracts.Models.ServiceDesk;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk
{
    public class ListItemModelProfile : Profile
    {
        public ListItemModelProfile()
        {
            CreateListItemModelMap<MyTasksReportItem>();
            CreateListItemModelMap<ObjectUnderControl>();
        }

        private IMappingExpression<T, ListItemModel> CreateListItemModelMap<T>()
            where T : ServiceDeskListItem
        {
            return CreateMap<T, ListItemModel>()
                .ForMember(model => model.PriorityColor, mapper => mapper.MapFrom<PriorityColorResolver>())
                .ForMember(
                    model => model.Uri,
                    mapper => mapper.MapFrom<
                            PathResolver<T, ListItemModel>,
                            InframanagerObject?>(
                                item => new InframanagerObject(item.ID, item.ClassID)))
                .ForMember(
                    model => model.IMObjID,
                    mapper => mapper.MapFrom(item => item.ID))
                .ForMember(model => model.PriorityColor, mapper => mapper.MapFrom<PriorityColorResolver>());
        }
    }
}
