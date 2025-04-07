using AutoMapper;
using IMSystem;
using InfraManager.DAL;
using InfraManager.DAL.OrganizationStructure;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices;

internal class ServiceCustomerProfile : Profile
{
    public ServiceCustomerProfile()
    {
        CreateMap<User, ServiceCustomerDetails>()
            .ForMember(dst => dst.ClassId, m => m.MapFrom(scr => Global.OBJ_USER))
            .ForMember(dst => dst.ID, m => m.MapFrom(scr => scr.IMObjID))
            .ForMember(dst => dst.FullName, m => m.MapFrom(scr => scr.Surname + " " + scr.Name + " " + scr.Patronymic))
            .ForMember(dst => dst.PositionName, m => m.MapFrom(scr => scr.Position.Name))
            ;

        CreateMap<Organization, ServiceCustomerDetails>()
            .ForMember(dst => dst.ClassId, m => m.MapFrom(scr => Global.OBJ_ORGANIZATION))
            ;

        CreateMap<Group, ServiceCustomerDetails>()
            .ForMember(dst => dst.ClassId, m => m.MapFrom(scr => Global.OBJ_QUEUE))
            ;

        CreateMap<Subdivision, ServiceCustomerDetails>()
            .ForMember(dst => dst.ClassId, m => m.MapFrom(scr => Global.OBJ_DIVISION))
            ;
    }
}
