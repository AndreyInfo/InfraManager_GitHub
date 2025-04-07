using System;
using AutoMapper;
using InfraManager.DAL.Location;

namespace InfraManager.BLL.Location.Workplaces;

internal class WorkplaceProfile : Profile
{
    public WorkplaceProfile()
    {
        CreateMap<Workplace, WorkplaceDetails>()
            .ForMember(dst => dst.ParentId, m => m.MapFrom(scr => scr.RoomID))
            .ForMember(dst => dst.ExternalID, m => m.MapFrom(scr => string.IsNullOrEmpty(scr.ExternalID) ? "" : scr.ExternalID))
            .ReverseMap()
            .ForMember(dst => dst.Room, m => m.Ignore())
            .ForMember(dst => dst.RoomID, m => m.MapFrom(scr => scr.ParentId));

        CreateMap<WorkplaceData, Workplace>()
            .ForMember(x => x.RoomID, x => x.MapFrom(x => x.ParentId));
    }
}
