using InfraManager.BLL.Location.Workplaces;
using InfraManager.BLL.OrganizationStructure.JobTitles;
using InfraManager.BLL.Roles;
using InfraManager.BLL.ServiceCatalogue;
using System;

namespace InfraManager.BLL.Users
{
    public class UserDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string Surname { get; set; }
        public string Initials { get; set; }
        public int? RoomID { get; set; }
        public int? PositionID { get; set; }
        public Guid? SubdivisionID { get; set; }
        public string Phone { get; set; }
        public string InPhone { get; set; }
        public string Phone2 { get; set; }
        public string Phone3 { get; set; }
        public string Phone4 { get; set; }
        public string Fax { get; set; }
        public string Pager { get; set; }
        public string Email { get; set; }
        public byte[] Photo { get; set; }
        public string Note { get; set; }
        public int? VisioId { get; set; }
        public string LoginName { get; set; }
        public string ExternalId { get; set; }
        public int? WorkplaceID { get; set; }
        public bool Admin { get; set; }
        public bool SupportOperator { get; set; }
        public bool NetworkAdmin { get; set; }
        public bool SupportEngineer { get; set; }
        public bool SupportAdmin { get; set; }
        public bool Removed { get; set; }
        public string Sid { get; set; }
        public Guid IMObjID { get; set; }
        public byte[] RowVersion { get; set; }
        public bool SdwebAccessIsGranted { get; set; }
        public byte[] SdwebPassword { get; set; }
        public Guid? PeripheralDatabaseId { get; set; }
        public int? ComplementaryId { get; set; }
        public Guid? ComplementaryGuidId { get; set; }
        public string Number { get; set; }
        public string TimeZoneId { get; set; }
        public Guid? CalendarWorkScheduleId { get; set; }
        public bool? IsLockedForOsi { get; set; }
        public Guid? ManagerId { get; set; }

        public JobTitleDetails Position { get; init; }
        public WorkplaceDetails Workplace { get; set; }
        public SubdivisionDTO Subdivision { get; set; }

        public RoleListItemDetails[] Roles { get; set; }
    }
}
