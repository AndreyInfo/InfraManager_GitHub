using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace InfraManager.DAL.Search
{
    internal class UserEmailSearchQuery : IUserEmailSearchQuery, ISelfRegisteredService<IUserEmailSearchQuery>
    {
        private readonly DbContext _db;

        public UserEmailSearchQuery(CrossPlatformDbContext db)
        {
            _db = db;
        }

        public IQueryable<ObjectSearchResult> Query(
            UserEmailSearchCriteria searchCriteria,
            Guid currentUserId)
        {
            var query = _db.Set<User>().AsNoTracking()
                .Where(User.ExceptSystemUsers)
                .Where(u => !u.Removed && !string.IsNullOrWhiteSpace(u.Email))
                .FilterUsers(_db, currentUserId);

            query = query.Include(u => u.Position);

            if (!string.IsNullOrWhiteSpace(searchCriteria.Text))
            {
                query = query.FilterUsers(searchCriteria.Text.ToLower().ToStartsWithPattern());
            }

            return query
                .Select(
                    u => new ObjectSearchResult
                    {
                        ID = u.IMObjID,
                        ClassID = ObjectClass.User,
                        FullName = User.GetFullName(u.IMObjID),
                        Details = $"{u.LoginName}, {u.PositionName}, {u.Email}"
                    });
        }
    }
}
