using System.Linq;
using AutoMapper;
using System;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.Location;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.Location;

internal sealed class LocationTreeNodeProfile : Profile
{
    public LocationTreeNodeProfile()
    {
        CreateMap<Owner, LocationTreeNodeDetails>()
             .ForMember(c => c.UID, m => m.MapFrom(scr => scr.IMObjID))
             .ForMember(c => c.ClassID, m => m.MapFrom(scr => ObjectClass.Owner))
             .ForMember(c => c.ParentUID, m => m.MapFrom(scr => Guid.Empty))
             .ForMember(c => c.ParentID, m => m.MapFrom(scr => 0))
             ;

        CreateMap<Organization, LocationTreeNodeDetails>()
           .ForMember(c => c.ClassID, m => m.MapFrom(scr => ObjectClass.Organizaton))
           .ForMember(c => c.HasChild, m => m.MapFrom(scr => scr.Buildings.Any()))
           .ForMember(c => c.UID, m => m.MapFrom(scr => scr.ID))
           .ForMember(c => c.ParentUID, m => m.MapFrom(scr => Guid.Empty))
           .ForMember(c => c.ParentID, m => m.Ignore())
           .ForMember(c => c.ID, m => m.Ignore())
           ;

        CreateMap<Building, LocationTreeNodeDetails>()
            .ForMember(c => c.ClassID, m => m.MapFrom(scr => ObjectClass.Building))
            .ForMember(c => c.HasChild, m => m.MapFrom(scr => scr.Floors.Any()))
            .ForMember(c => c.UID, m => m.MapFrom(scr => scr.IMObjID))
            .ForMember(c => c.ParentUID, m => m.MapFrom(scr => scr.OrganizationID.Value))
            .ForMember(c => c.ParentID, m => m.MapFrom(scr => 0))
            ;

        CreateMap<Floor, LocationTreeNodeDetails>()
            .ForMember(c => c.ClassID, m => m.MapFrom(scr => ObjectClass.Floor))
            .ForMember(c => c.HasChild, m => m.MapFrom(scr => scr.Rooms.Any()))
            .ForMember(c => c.UID, m => m.MapFrom(scr => scr.IMObjID))
            .ForMember(c => c.ParentID, m => m.MapFrom(scr => scr.Building.ID))
            .ForMember(c => c.ParentUID, m => m.MapFrom(scr => scr.Building.IMObjID))
            ;

        CreateMap<Room, LocationTreeNodeDetails>()
            .ForMember(c => c.ClassID, m => m.MapFrom(scr => ObjectClass.Room))
            .ForMember(c => c.HasChild, m => m.MapFrom(scr => scr.Workplaces.Any()))
            .ForMember(c => c.UID, m => m.MapFrom(scr => scr.IMObjID))
            .ForMember(c => c.ParentID, m => m.MapFrom(scr => scr.Floor.ID))
            .ForMember(c => c.ParentUID, m => m.MapFrom(scr => scr.Floor.IMObjID))
            ;

        CreateMap<Workplace, LocationTreeNodeDetails>()
            .ForMember(c => c.ClassID, m => m.MapFrom(scr => ObjectClass.Workplace))
            .ForMember(c => c.HasChild, m => m.MapFrom(scr => false))
            .ForMember(c => c.UID, m => m.MapFrom(scr => scr.IMObjID))
            .ForMember(c => c.ParentID, m => m.MapFrom(scr => scr.Room.ID))
            .ForMember(c => c.ParentUID, m => m.MapFrom(scr => scr.Room.IMObjID))
            ;

        CreateMap<Rack, LocationTreeNodeDetails>()
           .ForMember(c => c.ClassID, m => m.MapFrom(scr => ObjectClass.Rack))
           .ForMember(c => c.HasChild, m => m.MapFrom(scr => false))
           .ForMember(c => c.UID, m => m.MapFrom(scr => scr.IMObjID))
           .ForMember(c => c.ParentID, m => m.MapFrom(scr => scr.RoomID))
           .ForMember(c => c.ParentUID, m => m.MapFrom(scr => scr.Room.IMObjID))
           ;

        CreateMap<Rack, LocationTreeNodeDetails>()
           .ForMember(c => c.ClassID, m => m.MapFrom(scr => ObjectClass.Rack))
           .ForMember(c => c.HasChild, m => m.MapFrom(scr => false))
           .ForMember(c => c.UID, m => m.MapFrom(scr => scr.IMObjID))
           .ForMember(c => c.ParentID, m => m.MapFrom(scr => scr.RoomID))
           .ForMember(c => c.ParentUID, m => m.MapFrom(scr => scr.Room.IMObjID))
           ;

        CreateMap<LocationTreeNode, LocationTreeNodeDetails>();
    }
}
