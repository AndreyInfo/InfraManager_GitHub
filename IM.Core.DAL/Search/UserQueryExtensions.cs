using InfraManager.DAL.AccessManagement;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace InfraManager.DAL.Search
{
    internal static class UserQueryExtensions
    {
        internal static IQueryable<User> FilterUsers(this IQueryable<User> query, DbContext db, Guid currentUserId)
            => query.Where(User.ExceptSystemUsers)
                .Where(
                    u => db.Set<UserRole>().Any(x => x.UserID == currentUserId && x.RoleID == Role.AdminRoleId)
                        || db.Set<ObjectAccess>()
                            .Any(
                                x => x.OwnerID == u.IMObjID
                                    && x.Type == AccessTypes.TOZ_org
                                    && x.Propagate
                                    && x.ClassID == ObjectClass.Division
                                    && x.ObjectID == u.Subdivision.ID)
                        || db.Set<ObjectAccess>()
                            .Any(
                                x => x.OwnerID == u.IMObjID
                                    && x.Type == AccessTypes.TOZ_org
                                    && x.Propagate
                                    && x.ClassID == ObjectClass.Organizaton
                                    && x.ObjectID == u.Subdivision.Organization.ID)
                        || db.Set<ObjectAccess>()
                            .Any(
                                x => x.OwnerID == u.IMObjID
                                    && x.Type == AccessTypes.TOZ_org
                                    && x.Propagate
                                    && x.ClassID == ObjectClass.Owner)
                        || DbFunctions.AccessIsGranted(
                            ObjectClass.User,
                            u.IMObjID,
                            currentUserId,
                            ObjectClass.User,
                            AccessTypes.TOZ_org, false));

        internal static IQueryable<User> FilterUsers(this IQueryable<User> query, string pattern)
            => query.Where(u => EF.Functions.Like(u.Surname.Trim().ToLower() + " " + u.Name.Trim().ToLower() + " " + u.Patronymic.ToLower(), pattern)
                            || EF.Functions.Like(u.Number.ToLower(), pattern)
                            || EF.Functions.Like(u.Email.ToLower(), pattern)
                            || EF.Functions.Like(u.Phone.ToLower(), pattern)
                            || EF.Functions.Like(u.Phone1.ToLower(), pattern)
                            || EF.Functions.Like(u.LoginName.ToLower(), pattern));
    }
}
