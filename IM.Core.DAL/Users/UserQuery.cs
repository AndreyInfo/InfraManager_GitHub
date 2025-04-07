using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Users;

internal class UserQuery : IUserQuery, ISelfRegisteredService<IUserQuery>
{
    private readonly DbSet<User> _users;
    private readonly DbSet<UserRole> _userRoles;
    private readonly IPagingQueryCreator _pagging;

    public UserQuery(DbSet<User> users
                     , DbSet<UserRole> userRoles
                     , IPagingQueryCreator pagging)
    {
        _users = users;
        _userRoles = userRoles;
        _pagging = pagging;
    }

    public async Task<UserModelItem[]> ExecuteAsync(Guid objectID
        , OperationID[] operationIDs
        , ObjectClass objectClass
        , string searchString
        , Sort sortColumn
        , int take
        , int skip
        , bool? onlyWithEmails
        , bool kbExpert
        , CancellationToken cancellationToken)
    {

        var query = _users.Where(u => !u.Removed).Where(User.ExceptSystemUsers)
            .Include(c=> c.Subdivision)
            .ThenInclude(c => c.ParentSubdivision).AsNoTracking();
        
        query = objectClass switch
        {
            ObjectClass.Organizaton => query.Where(x => x.Subdivision.OrganizationID == objectID),
            ObjectClass.Division => query.Where(x => x.SubdivisionID == objectID),
            ObjectClass.Role => query.Where(x => _userRoles.Any(y => y.UserID == x.IMObjID && y.RoleID == objectID)),
            _ => query
        };

        if (operationIDs.Any())
            query = query.Where(c => _userRoles.Where(x => x.UserID == c.IMObjID)
                         .Any(x => x.Role.Operations.Any(v => operationIDs.Contains(v.OperationID))));

        if (!string.IsNullOrEmpty(searchString))
            query = query.Where(c =>
                    c.Name.ToLower().Contains(searchString.ToLower()) ||
                    c.Surname.ToLower().Contains(searchString.ToLower()) ||
                    c.Patronymic.ToLower().Contains(searchString.ToLower()) ||
                    c.Email.ToLower().Contains(searchString.ToLower()) ||
                    c.LoginName.ToLower().Contains(searchString.ToLower()) ||
                    c.Phone.ToLower().Contains(searchString.ToLower()) ||
                    c.Phone1.ToLower().Contains(searchString.ToLower()) ||
                    c.Number.ToLower().Contains(searchString.ToLower()) ||
                    c.Workplace.Name.ToLower().Contains(searchString.ToLower()));

        if (onlyWithEmails.HasValue)
        {
            query = query.Where(x => !string.IsNullOrEmpty(x.Email));
        }

        if (kbExpert)
        {
            query = query.Where(User.HasOperation(OperationID.BeKnowledgeBaseExpert));
        }
        
        var resultQuery = query.Select(c => new UserModelItem()
        {
            ID = c.IMObjID,
            IMObjID = c.IMObjID,
            Name = c.Name ?? string.Empty,
            SurName = c.Surname ?? string.Empty,
            Patronymic = c.Patronymic ?? string.Empty,
            Email = c.Email ?? string.Empty,
            Note = c.Note ?? string.Empty,
            Phone = c.Phone ?? string.Empty,
            Phone1 = c.Phone1 ?? string.Empty,
            LoginName = c.LoginName ?? string.Empty,
            Fax = c.Fax ?? string.Empty,
            SubdivisionID = c.SubdivisionID,
            Department = Subdivision.GetFullSubdivisionName(c.SubdivisionID),
            SubdivisionName = c.Subdivision.Name ?? string.Empty,
            OrganizationID = c.Subdivision.OrganizationID,
            Organization = c.Subdivision.Organization.Name ?? string.Empty,
            OrganizationName = c.Subdivision.Organization.Name ?? string.Empty,
            Building = c.Workplace.Room.Floor.Building.Name ?? string.Empty,
            BuildingName = c.Workplace.Room.Floor.Building.Name ?? string.Empty,
            Floor = c.Workplace.Room.Floor.Name ?? string.Empty,
            FloorName = c.Workplace.Room.Floor.Name ?? string.Empty,
            RoomName = c.Workplace.Room.Name ?? string.Empty,
            Room = c.Workplace.Room.Name ?? string.Empty,
            RoomID = c.Workplace.RoomID,
            WorkplaceID = c.WorkplaceID,
            WorkplaceIMObjID = c.Workplace.IMObjID,
            WorkplaceName = c.Workplace.Name ?? string.Empty,
            CalendarWorkScheduleId = c.CalendarWorkScheduleID,
            PositionName =  c.Position.Name ?? string.Empty,
            ExternalID = c.ExternalID,
            IsLockedForOsi = c.IsLockedForOsi.GetValueOrDefault(),
            ManagerID = c.ManagerID,
            HasAdminRole = c.Admin,
            WebAccessIsGranted = c.SDWebAccessIsGranted,
            TimeZoneId = c.TimeZoneID,
            Number = c.Number ?? string.Empty, 
            PhoneInternal = c.Phone1 ?? string.Empty,
            Family = c.Surname ?? string.Empty
        });
        
        var paggingQuery = _pagging.Create(resultQuery.OrderBy(sortColumn));
        return await paggingQuery.PageAsync(skip, take, cancellationToken);
    }
}
