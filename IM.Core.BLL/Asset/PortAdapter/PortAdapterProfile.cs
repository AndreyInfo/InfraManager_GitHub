using AutoMapper;
using InfraManager.BLL.ProductCatalogue.PortTemplates;
using InfraManager.DAL.Asset;
using PortAdapterEntity = InfraManager.DAL.Asset.PortAdapter;

namespace InfraManager.BLL.Asset.PortAdapter;

internal class PortAdapterProfile : Profile
{
    public PortAdapterProfile()
    {
        CreateMap<PortAdapterEntity, PortAdapterDetails>();
        CreateMap<PortAdapterData, PortAdapterEntity>();
    }
}