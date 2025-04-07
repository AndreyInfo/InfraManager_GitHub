using AutoMapper;
using InfraManager.DAL.Location;

namespace InfraManager.BLL.Location.Rooms;

internal class RoomProfile : Profile
{
    public RoomProfile()
    {
        CreateMap<Room, RoomDetails>()
            .ForMember(dst => dst.ParentID, m => m.MapFrom(scr => scr.FloorID))
            .ReverseMap()
            ;

        CreateMap<RoomData, Room>()
            .ForMember(x => x.FloorID, x => x.MapFrom(x => x.ParentID));
    }
}
