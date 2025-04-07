using AutoMapper;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.Software.SoftwareManufacturers;
internal class SoftwareManufacturerProfile : Profile
{
    public SoftwareManufacturerProfile()
    {
        CreateMap<Manufacturer, SoftwareManufacturerDetails>()
            .ForMember(dest => dest.ID, mapper => mapper.MapFrom(src => src.ImObjID));
    }
}
