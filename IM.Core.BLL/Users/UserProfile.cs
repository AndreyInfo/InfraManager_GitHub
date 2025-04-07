using AutoMapper;
using IMSystem;
using InfraManager.BLL.OrganizationStructure;
using InfraManager.DAL;
using InfraManager.DAL.Users;
using System;
using InfraManager.BLL.AutoMapper;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.ServiceDesk;

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
                    model => model.SubdivisionFullName,
                    map => map.MapFrom(user => user.SubdivisionName))
                .ForMember(
                    model => model.WorkplaceIMObjID,
                    map => map.MapFrom(user => user.Workplace != null ? (Guid?)user.Workplace.IMObjID : null))
                .ForMember(
                    model => model.WebAccessIsGranted,
                    map => map.MapFrom(user => user.SDWebAccessIsGranted))
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
                .ForMember(model => model.IMObjID, map => map.MapFrom(x => x.IMObjID))
                .ForMember(x => x.PhoneInternal, x => x.MapFrom(x => x.Phone1))
                .ForMember(x => x.ManagerID, x => x.MapFrom(x => x.ManagerID));
            
            CreateMap<User, UserListItem>()
                .ForMember(
                    model => model.ID,
                    map => map.MapFrom(user => user.IMObjID))
                .ForMember(
                    model => model.Name,
                    map => map.MapFrom(user => user.FullName))
                .ForMember(
                    model => model.Details,
                    map => map.MapFrom(user => user.Details));

            CreateMap<User, OrganizationStructureNodeModelDetails>()
                .ForMember(
                    model => model.Name,
                    map => map.MapFrom(user => user.FullName))
                .ForMember(
                    model => model.ClassID,
                    map => map.MapFrom(user => Global.OBJ_USER))
                .ForMember(
                    model => model.ObjectID,
                    map => map.MapFrom(user => user.IMObjID));

            CreateMap<UserData, User>()
                .BeforeMap(PrepopulateNullableFields)
                .ForMember(
                    model => model.Pager,
                    map => map.MapFrom(user => user.Other))
                .ForMember(
                    model => model.Phone1,
                    map => map.MapFrom(user => user.PhoneInternal))
                .ForMember(
                    model => model.SDWebAccessIsGranted,
                    map => map.MapFrom(user => user.WebAccessIsGranted))
                .ForMember(
                    model => model.ManagerID,
                    map => map.MapFrom(user => user.ManagerID))
                .ForMember(
                    x => x.SDWebPassword,
                    x =>
                    {
                        x.PreCondition(p => !string.IsNullOrEmpty(p.Password));
                        x.MapFrom<UserPasswordResolver, string>(x => x.Password);
                    });

            CreateMap<User, UserEmailDetails>();
            
            CreateMap<UserModelItem, UserDetailsModel>();
            
            CreateMap<UserExecutorListQueryResultItem, UserListItem>()
                .ForMember(
                    dst => dst.ID,
                    map => map.MapFrom(src => src.IMObjID));
        }


        internal class UserPasswordResolver : IMemberValueResolver<UserData, User, string, byte[]>
        {
            private readonly UserPasswordService _passwordService;

            public UserPasswordResolver(UserPasswordService passwordService)
            {
                _passwordService = passwordService;
            }

            public byte[] Resolve(UserData source, User destination, string sourceMember, byte[] destMember,
                ResolutionContext context)
            {
                return _passwordService.CalculatePassword(source.Password);
            }
        }

        private void PrepopulateNullableFields(UserData userData, User user)
        {
            if (user.TimeZoneID != null && userData.TimeZoneID == null)
            {
                userData.TimeZoneID = user.TimeZoneID;
            }

            if (user.ManagerID != null && userData.ManagerID == null)
            {
                userData.ManagerID = user.ManagerID;
            }
        
            if (user.IsLockedForOsi != null && userData.IsLockedForOsi == null)
            {
                userData.IsLockedForOsi = user.IsLockedForOsi;
            }

            if (user.CalendarWorkScheduleID != null && userData.CalendarWorkScheduleID == null)
            {
                userData.CalendarWorkScheduleID = user.CalendarWorkScheduleID;
            }

            if (user.LoginName != null && userData.LoginName == null)
            {
                userData.LoginName = user.LoginName;
            }

            if (user.Note != null && userData.Note == null)
            {
                userData.Note = user.Note;
            }

            if (user.Number != null && userData.Number == null)
            {
                userData.Number = user.Number;
            }

            if (user.Pager != null && userData.Other == null)
            {
                userData.Other = user.Pager;
            }

            if (userData.WebAccessIsGranted == null)
            {
                userData.WebAccessIsGranted = user.SDWebAccessIsGranted;
            }
        }
    }
}
