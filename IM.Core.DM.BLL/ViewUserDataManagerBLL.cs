using IM.Core.DM.BLL.Interfaces;
using IM.Core.DM.BLL.Interfaces.Models;
using InfraManager;
using InfraManager.DAL;
using System;
using System.Linq;

namespace IM.Core.DM.BLL
{
    internal class ViewUserDataManagerBLL : IViewUserDataManagerBLL, ISelfRegisteredService<IViewUserDataManagerBLL>
    {
        private readonly IReadonlyRepository<ViewUser> viewUserRepository;
        private readonly IReadonlyRepository<User> userRepository;

        public ViewUserDataManagerBLL(
                    IReadonlyRepository<ViewUser> viewUserRepository,
                    IReadonlyRepository<User> userRepository)
        {
            this.viewUserRepository = viewUserRepository;
            this.userRepository = userRepository;
        }

        public bool ExistsByID(Guid id)
        {
            return userRepository.Query().Any(x => x.IMObjID == id);
        }

        public bool ExistsByEmail(Guid id, string email)
        {
            return userRepository.Query().Any(x => x.IMObjID != id && x.Email != null && x.Email == email && x.Removed == false);
        }

        public ViewUserModel Get(Guid id)
        {
            var view =  viewUserRepository.Query()
                .FirstOrDefault(x => x.ID == id);

            return view == null ? null : Map(view);
        }

        public ViewUserModel Get(int id)
        {
            var view = viewUserRepository.Query()
                .FirstOrDefault(x => x.IntID == id);

            return view == null ? null : Map(view);
        }

        private ViewUserModel Map(ViewUser user) =>
            new ViewUserModel
            {
                BuildingID = user.BuildingID,
                Phone = user.Phone,
                Removed = user.Removed,
                BuildingName = user.BuildingName,
                CalendarWorkScheduleID = user.CalendarWorkScheduleID,
                CalendarWorkScheduleName = user.CalendarWorkScheduleName,
                DivisionID = user.DivisionID,
                DivisionName = user.DivisionName,
                Email = user.Email,
                ExternalID = user.ExternalID,
                Family = user.Family,
                Fax = user.Fax,
                FloorID = user.FloorID,
                FloorName = user.FloorName,
                ID = user.ID,
                IntID = user.IntID,
                IsLockedForOSI = user.IsLockedForOSI ?? false,
                Login = user.Login,
                ManagerID = user.ManagerID,
                ManagerName = user.ManagerName,
                Name = user.Name,
                Note = user.Note,
                Number = user.Number,
                OrganizationID = user.OrganizationID,
                OrganizationName = user.OrganizationName,
                Pager = user.Pager,
                Patronymic = user.Patronymic,
                PhoneInternal = user.PhoneInternal,
                PositionID = user.PositionID,
                PositionName = user.PositionName,
                RoomID = user.RoomID,
                RoomName = user.RoomName,
                RowVersion = user.RowVersion,
                SDWebAccessIsGranted = user.SDWebAccessIsGranted,
                SID = user.SID,
                TimeZoneID = user.TimeZoneID,
                TimeZoneName = user.TimeZoneName,
                WorkplaceID = user.WorkplaceID,
                WorkplaceName = user.WorkplaceName
            };
    }
}
