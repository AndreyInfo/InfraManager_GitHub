using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.DAL.AssetsManagement.Hardware;
using InfraManager.ResourcesArea;
using Global = InfraManager.Core.Global;

namespace InfraManager.BLL.AssetsManagement.Hardware;

public class HardwareListItemMappingProfile : Profile
{
    public HardwareListItemMappingProfile()
    {
        CreateMap<HardwareListQueryResultItemBase, HardwareListItem>()
            .ForMember(
                dst => dst.AppointmentDate,
                mapper => mapper.MapFrom(
                    src => DateTimeExtensions.Format(src.AssetItem.AppointmentDate, Global.DateFormat)))
            .ForMember(
                dst => dst.Warranty,
                mapper => mapper.MapFrom(
                    src => DateTimeExtensions.Format(src.Warranty, Global.DateFormat)))
            .ForMember(
                dst => dst.DateReceived,
                mapper => mapper.MapFrom(
                    src => DateTimeExtensions.Format(src.DateReceived, Global.DateFormat)))
            .ForMember(
                dst => dst.DateInquiry,
                mapper => mapper.MapFrom(
                    src => DateTimeExtensions.Format(src.DateInquiry, Global.DateTimeFormat)))
            .ForMember(
                dst => dst.DateAnnuled,
                mapper => mapper.MapFrom(
                    src => DateTimeExtensions.Format(src.DateAnnuled, Global.DateFormat)))
            .ForMember(
                dst => dst.UserField1,
                mapper => mapper.MapFrom(
                    src => src.AssetItem.UserField1))
            .ForMember(
                dst => dst.UserField2,
                mapper => mapper.MapFrom(
                    src => src.AssetItem.UserField2))
            .ForMember(
                dst => dst.UserField3,
                mapper => mapper.MapFrom(
                    src => src.AssetItem.UserField3))
            .ForMember(
                dst => dst.UserField4,
                mapper => mapper.MapFrom(
                    src => src.AssetItem.UserField4))
            .ForMember(
                dst => dst.UserField5,
                mapper => mapper.MapFrom(
                    src => src.AssetItem.UserField5))
            .ForMember(
                dst => dst.IsWorking,
                mapper => mapper.MapFrom<LocalizedTextResolver<HardwareListQueryResultItemBase, HardwareListItem>, string>(
                    src => src.IsWorking ? nameof(Resources.IsWorking_True) : nameof(Resources.IsWorking_False)))
            .ForMember(
                dst => dst.LocationOnStore,
                mapper => mapper.MapFrom<LocalizedTextResolver<HardwareListQueryResultItemBase, HardwareListItem>, string>(
                    src => src.LocationOnStore ? nameof(Resources.LocationOnStore_True) : nameof(Resources.LocationOnStore_False)))
            .ForMember(
                dst => dst.ServiceContractUtcFinishDate,
                mapper => mapper.MapFrom(
                    src => DateTimeExtensions.Format(
                        src.AssetItem.ServiceContractUtcFinishDate.HasValue ? src.AssetItem.ServiceContractUtcFinishDate.Value.ToLocalTime() : null, Global.DateFormat)));

        CreateMap<HardwareListQueryResultItemBase, AssetSearchListItem>()
            .ForMember(
                dst => dst.ServiceContractUtcFinishDate,
                mapper => mapper.MapFrom(
                    src => DateTimeExtensions.Format(
                        src.AssetItem.ServiceContractUtcFinishDate.HasValue ? src.AssetItem.ServiceContractUtcFinishDate.Value.ToLocalTime() : null, Global.DateFormat)));

        CreateMap<AllHardwareListQueryResultItem, HardwareListItem>()
            .IncludeBase<HardwareListQueryResultItemBase, HardwareListItem>();

        CreateMap<AssetSearchListQueryResultItem, AssetSearchListItem>()
            .IncludeBase<HardwareListQueryResultItemBase, AssetSearchListItem>();

        CreateMap<AllHardwareListQueryResultItem, ClientsHardwareListItem>()
            .IncludeBase<AllHardwareListQueryResultItem, HardwareListItem>();
    }
}