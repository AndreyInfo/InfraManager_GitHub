using AutoMapper;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.ServiceUnits;

internal class ServiceUnitProfile : Profile
{
    public ServiceUnitProfile()
    {
        CreateMap<ServiceUnit, ServiceUnitDetails>()
            .ForMember(dst => dst.ResponsibleName, m => m.MapFrom(c => c.ResponsibleUser.FullName))
            .ReverseMap()
            .ForMember(dst => dst.ResponsibleUser, m => m.Ignore());

        CreateMap<ServiceUnitInsertDetails, ServiceUnit>()
            .ForMember(dst => dst.ResponsibleUser, m => m.Ignore());
    }
}
