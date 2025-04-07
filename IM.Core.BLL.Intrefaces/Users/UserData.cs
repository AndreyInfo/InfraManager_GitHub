
using System;

namespace InfraManager.BLL.Users;

public class UserData
{
    public string Surname { get; init; }

    public string Name { get; init; }

    public string Patronymic { get; init; }

    public string Number { get; set; }

    public string ExternalID { get; init; }

    public string Other { get; set; }

    public Guid? CalendarWorkScheduleID { get; set; }

    public bool? WebAccessIsGranted { get; set; }

    public string Note { get; set; }

    public Guid? SubdivisionID { get; init; }

    public int? PositionID { get; init; }

    public int? WorkplaceID { get; init; }

    public int? RoomID { get; init; }

    public string TimeZoneID { get; set; }

    public string LoginName { get; set; }

    public string PhoneInternal { get; init; }

    public string Fax  { get; init; }

    public string Email { get; init; }

    public string Phone { get; init; }

    public bool? IsLockedForOsi { get; set; }

    public Guid? ManagerID { get; set; }
    
    public string Password { get; init; }
}
