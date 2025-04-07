using AutoMapper;
using InfraManager.BLL.ProductCatalogue.PortTemplates;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.ProductCatalogue.PortsTemplates;

public class PortTemplateProfile : Profile
{
    public PortTemplateProfile()
    {
        CreateMap<PortTemplatesKey, PortTemplatesData>();

        CreateMap<PortTemplatesData, PortTemplate>();

        CreateMap<PortTemplate, PortTemplatesDetails>()
            .ForMember(x => x.JackTypeName, x => x.MapFrom(y => y.JackType.Name))
            .ForMember(x => x.TechnologyTypeName, x => x.MapFrom(y => y.TechnologyType.Name));
    }
}