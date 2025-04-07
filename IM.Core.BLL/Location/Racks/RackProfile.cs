using AutoMapper;
using InfraManager.BLL.Asset.dto;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.Location.Racks
{
    public class RackProfile : Profile
    {
        public RackProfile()
        {
            CreateMap<RackData, Rack>();

            CreateMap<Rack, RackDetails>()
                .ForMember(
                    dto => dto.ID,
                    options => options.MapFrom(rack => rack.IMObjID))
                .ForMember(
                    dto => dto.IntID,
                    options => options.MapFrom(rack => rack.ID))
                .ForMember(
                    dto => dto.RoomID,
                    options => options.MapFrom(rack => rack.Room.IMObjID))
                .ForMember(
                    dto => dto.RoomIntID,
                    options => options.MapFrom(rack => rack.Room.ID))
                .ForMember(
                    dto => dto.RoomName,
                    options => options.MapFrom(rack => rack.Room.Name))
                .ForMember(
                    dto => dto.FloorID,
                    options => options.MapFrom(rack => rack.Floor.IMObjID))
                .ForMember(
                    dto => dto.FloorName,
                    options => options.MapFrom(rack => rack.Floor.Name))
                .ForMember(
                    dto => dto.BuildingID,
                    options => options.MapFrom(rack => rack.Building.IMObjID))
                .ForMember(
                    dto => dto.BuildingName,
                    options => options.MapFrom(rack => rack.Building.Name))
                .ForMember(
                    dto => dto.OrganizationID,
                    options => options.MapFrom(rack => rack.Building.Organization.ID))
                .ForMember(
                    dto => dto.OrganizationName,
                    options => options.MapFrom(rack => rack.Building.Organization.Name))
                .ReverseMap();
        }
    }
}
