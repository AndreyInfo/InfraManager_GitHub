using AutoMapper;
using InfraManager.DAL.ServiceCatalogue;

namespace InfraManager.BLL.ServiceCatalogue;

public class SLAReferenceProfile : Profile
{
    public SLAReferenceProfile()
    {
        CreateMap<SLAReference, SLAReferenceDetails>().ReverseMap();

        CreateMap<SLAReferenceData, SLAReference>();
    }
}