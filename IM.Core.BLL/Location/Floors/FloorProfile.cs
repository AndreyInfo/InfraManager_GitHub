using System;
using AutoMapper;
using InfraManager.DAL.Location;

namespace InfraManager.BLL.Location.Floors;

internal class FloorProfile : Profile
{
    public FloorProfile()
    {
        CreateMap<Floor, FloorDetails>()
            .ForMember(dst => dst.ParentID, m => m.MapFrom(scr => scr.BuildingID))
            .ReverseMap()
            .ForMember(dst => dst.BuildingID, m => m.MapFrom(scr => scr.ParentID))
            ;

        CreateMap<FloorData, Floor>()
            .ForMember(x => x.BuildingID, x => x.MapFrom(x => x.ParentID));
    }
}
