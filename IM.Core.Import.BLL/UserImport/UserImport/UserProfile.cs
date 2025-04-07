using AutoMapper;
using InfraManager.DAL;
using IMCore.Import.BLL.Interface.Authorization;

namespace InfraManager.BLL.Users
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDetailsModel>()
                .ForMember(
                    model => model.Other,
                    map => map.MapFrom(user => user.Pager ?? string.Empty))
                .ForMember(
                    model => model.Phone,
                    map => map.MapFrom(user => user.Phone ?? string.Empty))
                .ForMember(
                    model => model.PhoneInternal,
                    map => map.MapFrom(user => user.Phone1 ?? string.Empty))
                .ForMember(
                    model => model.ID,
                    map => map.MapFrom(user => user.IMObjID))
                .ForMember(
                    model => model.Family,
                    map => map.MapFrom(user => user.Surname))
                .ForMember(
                    model => model.WorkplaceID,
                    map => map.MapFrom(user => user.Workplace != null ? (int?)user.Workplace.ID : null))
                .ForMember(
                    model => model.WorkplaceIMObjID,
                    map => map.MapFrom(user => user.Workplace != null ? (Guid?)user.Workplace.IMObjID : null))
                .ForMember(
                    model => model.WebAccessIsGranted,
                    map => map.MapFrom(user => user.SDWebAccessIsGranted))
                .ForMember(
                    model => model.PositionID,
                    map => map.MapFrom(user => user.Position == null ? user.Position.IMObjID : (Guid?)null ))
                .ForMember(model => model.Building,
                    map => map.MapFrom(user => user.Workplace.Room.Floor.Building.Name))
                .ForMember(model => model.Floor,
                    map => map.MapFrom(user => user.Workplace.Room.Floor.Name))
                .ForMember(model => model.Room,
                    map => map.MapFrom(user => user.Workplace.Room.Name))
                .ForMember(model => model.Organization,
                    map => map.MapFrom(user => user.Subdivision.Organization.Name)) //TODO поменять на Workplace
                .ForMember(model => model.Department,
                    map => map.MapFrom(user => user.Subdivision.Name))
                .ForMember(model=>model.IMObjID, map => map.MapFrom(x=>x.IMObjID));

        }
    }
}
