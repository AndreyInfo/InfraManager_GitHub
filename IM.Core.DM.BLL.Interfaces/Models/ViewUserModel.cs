using System;

namespace IM.Core.DM.BLL.Interfaces.Models
{
    public class ViewUserModel
    {
        public int IntID { get; set; }

        public Guid? ID { get; set; }

        public string Family { get; set; }

        public string Name { get; set; }

        public string Patronymic { get; set; }

        public string Phone { get; set; }

        public string PhoneInternal { get; set; }

        public string Email { get; set; }

        public string Fax { get; set; }

        public string Pager { get; set; }

        public string Note { get; set; }

        public string Login { get; set; }

        public string SID { get; set; }

        public bool SDWebAccessIsGranted { get; set; }

        public byte[] RowVersion { get; set; }

        public bool Removed { get; set; }

        public int? RoomID { get; set; }

        public string RoomName { get; set; }

        public int? BuildingID { get; set; }

        public string BuildingName { get; set; }

        public int? FloorID { get; set; }

        public string FloorName { get; set; }

        public int? PositionID { get; set; }

        public string PositionName { get; set; }

        public Guid? DivisionID { get; set; }

        public string DivisionName { get; set; }

        public Guid? OrganizationID { get; set; }

        public string OrganizationName { get; set; }

        public Guid? WorkplaceID { get; set; }

        public string WorkplaceName { get; set; }

        public string Number { get; set; }

        public string TimeZoneID { get; set; }

        public string TimeZoneName { get; set; }

        public Guid? CalendarWorkScheduleID { get; set; }

        public string CalendarWorkScheduleName { get; set; }

        public string ExternalID { get; set; }

        public bool IsLockedForOSI { get; set; }

        public Guid? ManagerID { get; set; }

        public string ManagerName { get; set; }
    }
}
