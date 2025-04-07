using InfraManager;

namespace IMCore.Import.BLL.Interface.Authorization;

public class UserDetailsModel
{
    public string SurName { get; init; }
    public string Building { get; init; }
    public string Floor { get; init; }
    public string Room { get; init; }
    public string Organization { get; init; }
    public string Department { get; init; }
    public string Phone1 { get; init; }

    public string Phone { get; init; }
    public string PhoneInternal { get; init; }
    public bool CanCallToUser => false;
    public string Fax { get; init; }
    public string Other { get; init; }
    public string Avatar => string.Empty;
    public Guid? OrganizationID { get; init; }
    public string OrganizationName { get; init; }
    public Guid? SubdivisionID { get; init; }
    public string SubdivisionFullName { get; set; }
    public string SubdivisionName { get; init; }
    public string Number { get; init; }
    public string Note { get; init; }
    public string BuildingName { get; init; }
    public int? RoomID { get; init; }
    public string RoomName { get; init; }
    public string FloorName { get; init; }
    public int? WorkplaceID { get; init; }
    public Guid? WorkplaceIMObjID { get; init; }
    public string WorkplaceName { get; init; }
    public string WorkplaceFullName
    {
        get
        {
            return string.Join(" \\ ", new object[] { OrganizationName == string.Empty ? "Нет" : OrganizationName, BuildingName, FloorName, RoomName, WorkplaceName });
        }
    }
    public string FullName { get; init; }
    public string PositionName { get; init; }
    public UserDetailsModel[] UserDeputyList { get; set; }
    public Guid ID { get; init; }
    public Guid IMObjID { get; init; }
    public int ClassID => (int)ObjectClass.User;
    public string Family { get; init; }
    public string Name { get; init; }
    public string Patronymic { get; init; }
    public bool WebAccessIsGranted { get; init; }
    public string LoginName { get; init; }
    public string Email { get; init; }
    public bool HasAdminRole { get; set; }
    public OperationID[] GrantedOperations { get; set; }
    public bool HasRoles { get; set; }
    public Guid? CalendarWorkScheduleId { get; init; }
    public string TimeZoneId { get; init; }
    public bool IsLockedForOsi { get; init; }
    public int? PositionID { get; init; }
}