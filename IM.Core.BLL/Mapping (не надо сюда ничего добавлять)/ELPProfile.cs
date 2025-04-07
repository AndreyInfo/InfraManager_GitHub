using AutoMapper;
using InfraManager.BLL.ELP;
using InfraManager.CrossPlatform.WebApi.Contracts.ELP;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.Mapping
{
    public class ELPProfile : Profile
    {
        public ELPProfile()
        {
            CreateMap<ElpSetting, ELPSettingDetails>()
                .ForMember(dst => dst.Description, m => m.MapFrom(src => src.Note))
                .ForMember(dst => dst.ID, m => m.MapFrom(src => src.Id))
                .ForMember(dst => dst.Name, m => m.MapFrom(src => src.Name))
                .ForMember(dst => dst.VendorID, m => m.MapFrom(src => src.VendorId))
                .ForMember(dst => dst.VendorName, m => m.MapFrom(src => src.Vendor.Name));
            CreateMap<ELPItem, ElpSetting>()
                .ForMember(dst => dst.Note, m => m.MapFrom(src => src.Description))
                .ForMember(dst => dst.Id, m => m.MapFrom(src => src.ID))
                .ForMember(dst => dst.Name, m => m.MapFrom(src => src.Name))
                .ForMember(dst => dst.VendorId, m => m.MapFrom(src => src.VendorID));

            CreateMap<ELPSettingDetails, ELPListItem>();

            CreateMap<Manufacturer, ELPVendorListItem>()
                .ForMember(dst => dst.ID, m => m.MapFrom(src => src.ImObjID))
                .ForMember(dst => dst.Name, m => m.MapFrom(src => src.Name));

            AllowNullCollections = true;

        }
    }
}
