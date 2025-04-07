using AutoMapper;
using InfraManager.DAL.Asset;
using AssetEntity = InfraManager.DAL.Asset.Asset;

namespace InfraManager.BLL.Asset.Adapters;

public class AdapterProfile : Profile
{
    public AdapterProfile()
    {
        CreateMap<Adapter, AdapterDetails>()
        .ForMember(dest => dest.VendorName, mapper => mapper.MapFrom(src => src.Model.Vendor.Name))
        .ForMember(dest => dest.VendorID, mapper => mapper.MapFrom(src => src.Model.Vendor.ID))
        .ForMember(dest => dest.OrganizationName, mapper => mapper.MapFrom(src => src.Room.Floor.Building.Organization.Name))
        .ForMember(dest => dest.OrganizationID, mapper => mapper.MapFrom(src => src.Room.Floor.Building.Organization.ID))
        .ForMember(dest => dest.BuildingName, mapper => mapper.MapFrom(src => src.Room.Floor.Building.Name))
        .ForMember(dest => dest.BuildingID, mapper => mapper.MapFrom(src => src.Room.Floor.Building.ID))
        .ForMember(dest => dest.FloorName, mapper => mapper.MapFrom(src => src.Room.Floor.Name))
        .ForMember(dest => dest.FloorID, mapper => mapper.MapFrom(src => src.Room.Floor.ID))
        .ForMember(dest => dest.RoomName, mapper => mapper.MapFrom(src => src.Room.Name))
        .ForMember(dest => dest.RoomID, mapper => mapper.MapFrom(src => src.Room.ID))
        .ForMember(dest => dest.Classification, mapper => mapper.MapFrom(src => src.Model.ProductCatalogType
        .ProductCatalogCategory.Name + "/" + src.Model.ProductCatalogType.Name + "/" + src.Model.Name))
        .ForMember(dest => dest.TemplateID, mapper => mapper.MapFrom(src => src.Model.ProductCatalogType.ProductCatalogTemplateID));

        CreateMap<AssetEntity, AdapterDetails>()
        .ForMember(dest => dest.ComplementaryID, mapper => mapper.MapFrom(src => src.ComplementaryGuidID))
        .ForMember(dest => dest.ComplementaryIntID, mapper => mapper.MapFrom(src => src.ComplementaryID));

        CreateMap<AssetDetails, AdapterDetails>()
        .ForMember(dest => dest.ComplementaryID, mapper => mapper.MapFrom(src => src.ComplementaryGuidID))
        .ForMember(dest => dest.ComplementaryIntID, mapper => mapper.MapFrom(src => src.ComplementaryID));

        CreateMap<AdapterData, Adapter>();

        CreateMap<AdapterData, AssetData>();

        CreateMap<Adapter, AssetData>()
        .ForMember(dest => dest.ID, mapper => mapper.MapFrom(src => src.IMObjID))
        .ForMember(dest => dest.DeviceID, mapper => mapper.MapFrom(src => src.ID));
    }
}