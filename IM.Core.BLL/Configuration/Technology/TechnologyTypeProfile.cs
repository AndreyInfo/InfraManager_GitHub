using AutoMapper;
using InfraManager.DAL.Configuration;
using InfraManager.BLL.Technologies;

namespace InfraManager.BLL.Configuration.Technology;

internal sealed class TechnologyTypeProfile : Profile
{
    public TechnologyTypeProfile()
    {
        CreateMap<TechnologyType, TechnologyTypeDetails>();
        
        CreateMap<TechnologyTypeData, TechnologyType>();
    }
}
