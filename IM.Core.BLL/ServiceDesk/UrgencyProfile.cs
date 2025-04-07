using AutoMapper;
using InfraManager.BLL.ServiceDesk.DTOs;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk
{
    public class UrgencyProfile : Profile
    {
        public UrgencyProfile()
        {
            CreateMap<Urgency, UrgencyDTO>().ReverseMap();

            CreateMap<Urgency, UrgencyListItemModel>();                
        }
    }
}
