using Inframanager;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.Location;
using InfraManager.DAL.OrganizationStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TimeZone = InfraManager.DAL.ServiceDesk.TimeZone;

namespace InfraManager.DAL;

[ObjectClassMapping(ObjectClass.User)]
[OperationIdMapping(ObjectAction.Delete, OperationID.User_Delete)]
[OperationIdMapping(ObjectAction.Insert, OperationID.User_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.User_Update)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.User_View)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.User_View)]
public class User : ITimeZoneObject, AccessManagement.IUser, IGloballyIdentifiedEntity, IMarkableForDelete, ILockableForOsi
{
    #region .ctor

    protected User()
    {
    }


    public User(string name, string surname)
    {
        Name = name;
        Surname = surname;
        IMObjID = Guid.NewGuid();
        WorkplaceID = 0;
        RoomID = 0;
    }

    public User(Guid guid)
    {
        IMObjID = guid;
    }

    #endregion

    #region Identification

    public const int SystemUserId = 0;
    public static readonly Guid SystemUserGlobalIdentifier = Guid.Empty;
    public const int NullUserId = 1;
    public static readonly Guid NullUserGloablIdentifier = new ("00000000-0000-0000-0000-000000000001");

    public int ID { get; init; }
    public Guid IMObjID { get; init; }
    public byte[] RowVersion { get; init; }
    public int? PositionID { get; init; }

    public int? VisioID { get; init; }
    public string ExternalID { get; init; }
    public string SID { get; init; }
    public string Number { get; init; }

    public static int[] SystemUserIds = new[] { SystemUserId, NullUserId };

    public static Specification<User> ExceptSystemUsers =>
        new Specification<User>(user => !SystemUserIds.Contains(user.ID));

    public static Specification<User> ExceptRemovedUsers =>
        new Specification<User>(user => !user.Removed);

    #endregion

    #region Details

    public string Name { get; init; }
    public string Patronymic { get; init; }
    public string Surname { get; init; }
    public string Initials { get; init; }
    public byte[] Photo { get; init; }
    public string Note { get; init; }

    private static Func<User, string> _fullNameFunction = FullNameExpression.Compile();
    public static Expression<Func<User, string>> FullNameExpression => u => u.Surname + " " + u.Name + " " + u.Patronymic;
    public string FullName => _fullNameFunction(this);
    public string Details =>
        string.Join(
            ",",
            (new[]
            {
                        Number,
                        LoginName,
                        Phone,
                        Phone1,
                        Email
            })
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToArray());

    public static string GetFullName(Guid? userId) => throw new NotSupportedException();

    #endregion

    #region Contact Info

    public string Phone { get; init; }
    public string Phone1 { get; init; }
    public string Phone2 { get; init; }
    public string Phone3 { get; init; }
    public string Phone4 { get; init; }
    public string Fax { get; init; }
    public string Pager { get; init; }
    public string Email { get; init; }

    #endregion

    #region Job Info

    public int? RoomID { get; set; }

    public virtual JobTitle Position { get; }
    public Guid? SubdivisionID { get; init; }
    public virtual Subdivision Subdivision { get; }
    
    public int? WorkplaceID { get; set; }
    public virtual Workplace Workplace { get; }
    public string FloorName => Workplace?.Room?.Floor?.Name ?? string.Empty;
    public string BuildingName => Workplace?.Room?.Floor?.Building?.Name ?? string.Empty;
    public string RoomName => Workplace?.Room?.Name ?? string.Empty;
    public string WorkplaceName => Workplace?.Name ?? string.Empty;
    public Guid? OrganizationId => Subdivision?.OrganizationID;
    public string SubdivisionName => Subdivision?.Name ?? string.Empty;
    public string OrganizationName => Subdivision?.Organization?.Name ?? string.Empty;
    public string PositionName => Position?.Name ?? string.Empty;

    #endregion

    #region Access Info

    public string LoginName { get; init; }
    public bool Admin { get; init; }
    public bool SupportOperator { get; init; }
    public bool NetworkAdmin { get; init; }
    public bool SupportEngineer { get; init; }
    public bool SupportAdmin { get; init; }
    public bool SDWebAccessIsGranted { get; init; }
    public byte[] SDWebPassword { get; set; }

    #endregion

    #region IMarkableForDelete

    public bool Removed { get; set; }

    public void MarkForDelete()
    {
        Removed = true;
    }

    #endregion

    #region Other

    public Guid? PeripheralDatabaseId { get; init; }
    public int? ComplementaryId { get; init; }
    public Guid? ComplementaryGuidId { get; init; }
    public Guid? CalendarWorkScheduleID { get; init; }
    public bool? IsLockedForOsi { get; set; }
    public Guid? ManagerID { get; init; }
    public string TimeZoneID { get; set; }
    
    public virtual User?  Manager { get; set; }
    public virtual TimeZone TimeZone { get; init; }

    #endregion

    #region Roles

    public static Specification<User> HasNonAdminRole => 
        new Specification<User>(user => user.UserRoles.Any(x => x.RoleID != Role.AdminRoleId));
    public static Specification<User> IsAdmin =>
        new Specification<User>(user => user.UserRoles.Any(x => x.RoleID == Role.AdminRoleId));

    public static Specification<User> HasOperation(OperationID operationID) =>
        new Specification<User>(user => user.UserRoles.Any(r => r.Role.Operations.Any(x => x.OperationID == operationID)));

    public virtual ICollection<UserRole> UserRoles { get; } = new HashSet<UserRole>();

    #endregion
}
