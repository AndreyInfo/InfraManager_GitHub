using AutoMapper;
using InfraManager.BLL.Asset.dto;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.Mapping
{
    internal class CriticalityProfile : Profile
    {
        public CriticalityProfile()
        {
            CreateMap<Criticality, CriticalityDTO>(); 
            CreateMap<CriticalityDTO, Criticality>();


            CreateMap<CartridgeType, CartridgeTypeDTO>()
               .ReverseMap();

            CreateMap<SlotType, SlotTypeDTO>()
               .ReverseMap();

            CreateMap<FileSystem, FileSystemDTO>()
               .ReverseMap();
        }
    }
}
