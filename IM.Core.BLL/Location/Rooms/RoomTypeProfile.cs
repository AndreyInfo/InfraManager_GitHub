using AutoMapper;
using InfraManager.DAL.Location;

namespace InfraManager.BLL.Location.Rooms;

internal class RoomTypeProfile : Profile
{
    public RoomTypeProfile()
    {
        CreateMap<RoomType, RoomTypeDetails>()
            .ReverseMap()
            ;
    }
}
