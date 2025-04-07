using AutoMapper;
using InfraManager.DAL.Location;

namespace InfraManager.BLL.Location.StorageLocations;

internal sealed class StorageLocationProfile : Profile
{
    public StorageLocationProfile()
    {
        CreateMap<StorageLocation, StorageLocationDetails>()
            .ForMember(dst => dst.MOL, m => m.MapFrom(scr => scr.User != null ? scr.User.FullName : string.Empty))
            .ReverseMap()
            .ForMember(dst => dst.User, m => m.Ignore());

        CreateMap<StorageLocationInsertDetails, StorageLocation>()
            .ConstructUsing(c => new(c.Name, c.ExternalID, c.UserID));
    }
}
