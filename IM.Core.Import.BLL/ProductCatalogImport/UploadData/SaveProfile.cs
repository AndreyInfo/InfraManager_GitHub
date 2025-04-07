using AutoMapper;
using InfraManager.DAL.Asset;
using InfraManager.DAL.ProductCatalogue;

namespace IM.Core.Import.BLL.Import.Importer.UploadData;

public class SaveProfile : Profile
{
    public SaveProfile()
    {
        CreateMap<Manufacturer, Manufacturer>()
            .ForMember(x => x.ID, x => x.Ignore());
        
        CreateMap<ProductCatalogType, ProductCatalogType>()
            .ForMember(x => x.IMObjID, x => x.Ignore());
        
        CreateMap<AdapterType, AdapterType>()
            .ForMember(x => x.IMObjID, x => x.Ignore());
        
        CreateMap<PeripheralType, PeripheralType>()
            .ForMember(x => x.IMObjID, x => x.Ignore());

        CreateMap<TerminalDeviceModel, TerminalDeviceModel>()
            .ForMember(x => x.IMObjID, x => x.Ignore());

        CreateMap<SoftwareLicenseModel, SoftwareLicenseModel>()
            .ForMember(x => x.IMObjID, x => x.Ignore());

        CreateMap<MaterialModel, MaterialModel>()
            .ForMember(x => x.IMObjID, x => x.Ignore());
    }
}