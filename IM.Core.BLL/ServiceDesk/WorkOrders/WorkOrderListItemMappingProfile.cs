using AutoMapper;
using InfraManager.BLL.AutoMapper;

namespace InfraManager.BLL.ServiceDesk.WorkOrders
{
    public class WorkOrderListItemMappingProfile : Profile
    {
        public WorkOrderListItemMappingProfile()
        {
            CreateMap<DAL.ServiceDesk.WorkOrderListQueryResultItem, WorkOrderListItem>()
                .ForMember(m => m.ManhoursInMinutes, mapper =>
                        mapper.MapFrom<
                            ManhoursResolver<DAL.ServiceDesk.WorkOrderListQueryResultItem, WorkOrderListItem>,
                            int>(
                            item => item.Manhours))
                .ForMember(m => m.ManhoursNormInMinutes, mapper =>
                        mapper.MapFrom<
                            ManhoursResolver<DAL.ServiceDesk.WorkOrderListQueryResultItem, WorkOrderListItem>,
                            int>(
                            item => item.ManhoursNorm));
            
            CreateMap<DAL.ServiceDesk.WorkOrderListQueryResultItem, ReferencedWorkOrderListItem>()
                .ForMember(m => m.ManhoursInMinutes, mapper =>
                    mapper.MapFrom<
                        ManhoursResolver<DAL.ServiceDesk.WorkOrderListQueryResultItem, ReferencedWorkOrderListItem>,
                        int>(
                        item => item.Manhours))
                .ForMember(m => m.ManhoursNormInMinutes, mapper =>
                    mapper.MapFrom<
                        ManhoursResolver<DAL.ServiceDesk.WorkOrderListQueryResultItem, ReferencedWorkOrderListItem>,
                        int>(
                        item => item.ManhoursNorm));

            CreateMap<DAL.ServiceDesk.InventoryListQueryResultItem, InventoryListItem>()
                .ForMember(m => m.ManhoursInMinutes, mapper =>
                        mapper.MapFrom<
                            ManhoursResolver<DAL.ServiceDesk.InventoryListQueryResultItem, InventoryListItem>,
                            int>(
                            item => item.Manhours))
                .ForMember(m => m.ManhoursNormInMinutes, mapper =>
                        mapper.MapFrom<
                            ManhoursResolver<DAL.ServiceDesk.InventoryListQueryResultItem, InventoryListItem>,
                            int>(
                            item => item.ManhoursNorm)); 
        }
    }
}
