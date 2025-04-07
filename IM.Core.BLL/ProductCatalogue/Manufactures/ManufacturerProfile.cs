using AutoMapper;
using InfraManager.BLL.Software.SoftwareManufacturers;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.ProductCatalogue.Manufactures;

internal sealed class ManufacturerProfile : Profile
{
    public ManufacturerProfile()
    {
        CreateMap<ManufacturerData, Manufacturer>();

        CreateMap<Manufacturer, ManufacturerDetails>();
    }
}