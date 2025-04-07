using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceCatalog;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices;

internal sealed class SupportLineResponsibleProfile : Profile
{
    public SupportLineResponsibleProfile()
    {
        // Поле Line заполняется отдельно

        CreateMap<User, SupportLineResponsibleDetails>()
            .ForMember(dst => dst.Name, m => m.MapFrom(src => src.FullName))
            .ForMember(dst => dst.Id, m => m.MapFrom(scr => scr.IMObjID))
            .ForMember(dst => dst.ClassId, m => m.MapFrom(scr => ObjectClass.User))
            .ForMember(dst => dst.Line, m => m.Ignore())
            ;

        CreateMap<Group, SupportLineResponsibleDetails>()
            .ForMember(dst => dst.Id, m => m.MapFrom(scr => scr.IMObjID))
            .ForMember(dst => dst.ClassId, m => m.MapFrom(scr => ObjectClass.Group))
            .ForMember(dst => dst.Line, m => m.Ignore())
            ;

        CreateMap<Subdivision, SupportLineResponsibleDetails>()
            .ForMember(dst => dst.Id, m => m.MapFrom(scr => scr.ID))
            .ForMember(dst => dst.ClassId, m => m.MapFrom(scr => ObjectClass.Division))
            .ForMember(dst => dst.Line, m => m.Ignore())
            ;

        CreateMap<Organization, SupportLineResponsibleDetails>()
            .ForMember(dst => dst.Id, m => m.MapFrom(scr => scr.ID))
            .ForMember(dst => dst.ClassId, m => m.MapFrom(scr => ObjectClass.Organizaton))
            .ForMember(dst => dst.Line, m => m.Ignore())
            ;

        CreateMap<ServiceUnit, SupportLineResponsibleDetails>()
            .ForMember(dst => dst.Id, m => m.MapFrom(scr => scr.ID))
            .ForMember(dst => dst.ClassId, m => m.MapFrom(scr => ObjectClass.ServiceUnit))
            .ForMember(dst => dst.Line, m => m.Ignore())
            ;


        CreateMap<SupportLineResponsibleDetails, SupportLineResponsible>()
            .ForMember(dst => dst.OrganizationItemID, m => m.MapFrom(scr => scr.Id))
            .ForMember(dst => dst.OrganizationItemClassID, m => m.MapFrom(scr => scr.ClassId))
            .ForMember(dst => dst.LineNumber, m => m.MapFrom(scr => scr.Line))
            ;

        CreateMap<SupportLineResponsibleModelItem, SupportLineResponsibleDetails>();

        CreateMap<SupportLineResponsibleData, SupportLineResponsible>();
    }
}
