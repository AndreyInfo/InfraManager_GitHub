using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace InfraManager.DAL.Search
{
    internal class GroupSearchQuery : IGroupSearchQuery, ISelfRegisteredService<IGroupSearchQuery>
    {
        private readonly DbSet<Group> _groups;
        private readonly DbSet<GroupUser> _groupUsers;

        public GroupSearchQuery(DbSet<Group> groups, DbSet<GroupUser> groupUsers)
        {
            _groups = groups;
            _groupUsers = groupUsers;
        }

        public IQueryable<ObjectSearchResult> Query(GroupSearchCriteria criteria, Guid? userId = default)
        {
            var query = _groups.AsNoTracking()
                .Where(Group.IsNotNullObject)
                .Where(
                    g => _groupUsers.Any(x => x.GroupID == g.IMObjID
                        && userId.HasValue
                        && DbFunctions.AccessIsGranted(ObjectClass.User, x.UserID, userId.Value, ObjectClass.User, AccessManagement.AccessTypes.TOZ_sks, false)
                        && DbFunctions.AccessIsGranted(ObjectClass.User, x.UserID, userId.Value, ObjectClass.User, AccessManagement.AccessTypes.TOZ_org, false)));

            if (!string.IsNullOrWhiteSpace(criteria.Text))
            {
                var pattern = criteria.Text.ToContainsPattern();
                query = query.Where(
                    g => EF.Functions.Like(g.Name.ToLower(), pattern.ToLower())
                        || EF.Functions.Like(g.Note.ToLower(), pattern.ToLower()));
            }

            query = query.Where(x => x.Type != GroupType.None && criteria.Type == byte.MaxValue
                                     || ((byte) x.Type & criteria.Type) == criteria.Type);

            return query.Select(
                group => new ObjectSearchResult
                {
                    ID = group.IMObjID,
                    ClassID = ObjectClass.Group,
                    FullName = "[" + group.Name + "]" // TODO: Это логика отображения данных (не должно быть в DAL)
                });
        }
    }
}
