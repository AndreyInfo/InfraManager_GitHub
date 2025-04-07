using System;
using System.Linq;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.Search;

// TODO: Отрефакторить этот класс, 4 квери в одном
internal class UtilizerSearchQuery : IUtilizerSearchQuery, ISelfRegisteredService<IUtilizerSearchQuery>
{
    private readonly DbSet<Organization> _organizations;
    private readonly DbSet<Subdivision> _subdivisions;
    private readonly DbSet<User> _users;
    private readonly DbSet<Group> _groups;

    public UtilizerSearchQuery(
        DbSet<Organization> organizations,
        DbSet<Subdivision> subdivisions,
        DbSet<User> users,
        DbSet<Group> groups)
    {
        _organizations = organizations;
        _subdivisions = subdivisions;
        _users = users;
        _groups = groups;
    }

    public IQueryable<ObjectSearchResult> Query(UtilizerSearchCriteria searchBy, Guid currentUserId = default)
    {
        var searchPattern = string.IsNullOrWhiteSpace(searchBy.Text) ? null : searchBy.Text.ToContainsPattern();

        var organizations = GetOrganizations(searchPattern, currentUserId)
            .Select(o => new ObjectSearchResult
            {
                ID = o.ID,
                ClassID = ObjectClass.Organizaton,
                FullName = DbFunctions.CastAsString(o.Name),
            });

        var subdivisions = GetSubdivisions(searchPattern, currentUserId)
            .Select(s => new ObjectSearchResult
            {
                ID = s.ID,
                ClassID = ObjectClass.Division,
                FullName = DbFunctions.CastAsString(s.Name + "\\" + Subdivision.GetFullSubdivisionName(s.ID)),
            });

        var users = GetUsers(searchPattern, currentUserId)
            .Select(u => new ObjectSearchResult
            {
                ID = u.IMObjID,
                ClassID = ObjectClass.User,
                FullName = User.GetFullName(u.IMObjID),
            });

        var groups = GetGroups(searchPattern)
            .Select(g => new ObjectSearchResult
            {
                ID = g.IMObjID,
                ClassID = ObjectClass.Group,
                FullName = DbFunctions.CastAsString(g.Name),
            });

        return organizations.Concat(subdivisions).Concat(users).Concat(groups);
    }

    private IQueryable<Organization> GetOrganizations(string searchPattern, Guid userId)
    {
        var organizations = _organizations.AsNoTracking()
            .Where(o => DbFunctions.AccessIsGranted(
                ObjectClass.Organizaton,
                o.ID,
                userId,
                ObjectClass.User,
                AccessTypes.TOZ_org,
                false));

        if (searchPattern == null)
        {
            return organizations;
            
        }
        
        return organizations.Where(o => 
            EF.Functions.Like(o.Name, searchPattern)
            || EF.Functions.Like(o.Note, searchPattern));
    }

    private IQueryable<Subdivision> GetSubdivisions(string searchPattern, Guid userId)
    {
        var subdivisions = _subdivisions.AsNoTracking()
            .Where(s => DbFunctions.AccessIsGranted(
                ObjectClass.Division,
                s.ID,
                userId,
                ObjectClass.User,
                AccessTypes.TOZ_org,
                false));

        if (searchPattern == null)
        {
            return subdivisions;
        }
        
        return subdivisions.Where(s =>
            EF.Functions.Like(s.Name, searchPattern)
            || EF.Functions.Like(s.Note, searchPattern)
            || EF.Functions.Like(s.Name + "\\" + Subdivision.GetFullSubdivisionName(s.ID), searchPattern));
    }

    private IQueryable<User> GetUsers(string searchPattern, Guid userId)
    {
        var users = _users.Include(u => u.Position).AsNoTracking()
            .Where(User.ExceptSystemUsers)
            .Where(u => !u.Removed)
            .Where(u => DbFunctions.AccessIsGranted(
                ObjectClass.User,
                u.IMObjID,
                userId,
                ObjectClass.User,
                AccessTypes.TOZ_org,
                false));

        if (searchPattern == null)
        {
            return users;
        }

        return users.Where(u =>
            EF.Functions.Like(User.GetFullName(u.IMObjID), searchPattern)
            || EF.Functions.Like(u.Number, searchPattern)
            || EF.Functions.Like(u.LoginName, searchPattern)
            || EF.Functions.Like(u.Position.Name, searchPattern)
            || EF.Functions.Like(u.Phone, searchPattern)
            || EF.Functions.Like(u.Phone1, searchPattern)
            || EF.Functions.Like(u.Fax, searchPattern)
            || EF.Functions.Like(u.Pager, searchPattern)
            || EF.Functions.Like(u.Email, searchPattern)
            || EF.Functions.Like(u.Note, searchPattern));
    }

    private IQueryable<Group> GetGroups(string searchPattern)
    {
        var groups = _groups.AsNoTracking()
            .Where(Group.IsNotNullObject);

        if (searchPattern == null)
        {
            return groups;
        }

        return groups.Where(g =>
            EF.Functions.Like(g.Name, searchPattern)
            || EF.Functions.Like(g.Note, searchPattern));
    }
}