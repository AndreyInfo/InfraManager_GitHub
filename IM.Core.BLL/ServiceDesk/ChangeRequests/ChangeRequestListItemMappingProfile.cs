using AutoMapper;
using InfraManager.DAL.ServiceDesk.ChangeRequests;

namespace InfraManager.BLL.ServiceDesk.ChangeRequests
{
    public class ChangeRequestListItemMappingProfile : Profile
    {
        public ChangeRequestListItemMappingProfile()
        {
            CreateMap<ChangeRequestQueryResultItem, ChangeRequestListItem>()
                .ForMember(m => m.ManhoursInMinutes, m => m.MapFrom(x => x.Manhours))
                .ForMember(m => m.ManhoursNormInMinutes, m => m.MapFrom(x => x.ManhoursNorm))
                .ForMember(m => m.CategoryFullName, m => m.MapFrom(x => x.CategoryName));
        }
    }
}
