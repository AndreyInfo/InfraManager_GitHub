using AutoMapper;
using InfraManager.BLL.AutoMapper;
using System;
using AssetEntity = InfraManager.DAL.Asset.Asset;

namespace InfraManager.BLL.Asset;
internal class AssetProfile : Profile
{
    public AssetProfile()
    {
        CreateMap<AssetData, AssetEntity>()
            .ForMember(dest => dest.AppointmentDate, mapper => mapper.MapFrom(src => src.AppointmentDate ?? DateTime.UtcNow));

        CreateMap<AssetEntity, AssetData>();

        CreateMap<AssetEntity, AssetDetails>();
    }
}
