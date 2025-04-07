using AutoMapper;
using InfraManager.DAL.Location;
using System;

namespace InfraManager.BLL.Location.Buildings;

internal class BuildingProfile : Profile
{
    public BuildingProfile()
    {
        CreateMap<Building, BuildingDetails>()
            .ForMember(dst => dst.Index, m => m.MapFrom(scr => string.IsNullOrEmpty(scr.Index) ? string.Empty : scr.Index))
            .ForMember(dst => dst.Region, m => m.MapFrom(scr => string.IsNullOrEmpty(scr.Region) ? string.Empty : scr.Region))
            .ForMember(dst => dst.Street, m => m.MapFrom(scr => string.IsNullOrEmpty(scr.Street) ? string.Empty : scr.Street))
            .ForMember(dst => dst.House, m => m.MapFrom(scr => string.IsNullOrEmpty(scr.House) ? string.Empty : scr.House))
            .ForMember(dst => dst.Housing, m => m.MapFrom(scr => string.IsNullOrEmpty(scr.Housing) ? string.Empty : scr.Housing))
            .ForMember(dst => dst.HousePart, m => m.MapFrom(scr => string.IsNullOrEmpty(scr.HousePart) ? string.Empty : scr.HousePart));

        CreateMap<BuildingData, Building>();
    }
}
