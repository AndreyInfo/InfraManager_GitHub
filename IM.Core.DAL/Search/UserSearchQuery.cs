using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ServiceDesk;
using InfraManager.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace InfraManager.DAL.Search
{
    internal class UserSearchQuery : IUserSearchQuery, ISelfRegisteredService<IUserSearchQuery>
    {
        private readonly DbContext _db;
        private readonly IBuildSearchSpecification<User> _searchSpecBuilder;

        public UserSearchQuery(
            CrossPlatformDbContext db, 
            IBuildSearchSpecification<User> searchSpecBuilder)
        {
            _db = db;
            _searchSpecBuilder = searchSpecBuilder;
        }

        public IQueryable<ObjectSearchResult> Query(
            UserSearchCriteria searchCriteria,
            Guid currentUserId)
        {
            var query = _db.Set<User>().AsNoTracking().Where(u => !u.Removed).Where(User.ExceptSystemUsers);

            if (!searchCriteria.NoTOZ && searchCriteria.UseTTZ)
            {
                query = query.FilterUsers(_db, currentUserId);
            }

            if (!string.IsNullOrWhiteSpace(searchCriteria.Text))
            {
                query = query.Where(_searchSpecBuilder.Build(searchCriteria.Text));
            }

            if (searchCriteria.Operations.Any())
            {
                var userOperations = from roleOperation in _db.Set<RoleOperation>()
                                     join userRole in _db.Set<UserRole>()
                                        on roleOperation.RoleID equals userRole.RoleID
                                     select new { userRole.UserID, roleOperation.OperationID };
                var operationIds = searchCriteria.Operations;
                query = query
                    .Where(
                        u => userOperations
                            .Any(
                                x => x.UserID == u.IMObjID
                                    && operationIds.Contains(x.OperationID)));
            }

            if (searchCriteria.QueueId.HasValue)
            {
                query = query.Where(u => _db.Set<GroupUser>()
                        .Any(x => x.UserID == u.IMObjID
                                && x.GroupID == searchCriteria.QueueId.Value));
            }

            if (searchCriteria.HasAnyNonAdministrativeRole)
            {
                query = query.Where(
                    u => _db.Set<UserRole>()
                        .Any(x => x.UserID == u.IMObjID && x.RoleID != Role.AdminRoleId));
            }

            if (searchCriteria.UserId.HasValue)
            {
                var user = _db.Set<User>()
                    .Include(x => x.Subdivision)
                    .Single(x => x.IMObjID == searchCriteria.UserId.Value);
                var subdivisionId = user.Subdivision?.ID;
                query = query.Where(
                    u => u.IMObjID == searchCriteria.UserId.Value
                        || Subdivision.SubdivisionIsSibling(subdivisionId, u.Subdivision.ID));
            }

            if(searchCriteria.ExceptUserIDs.Length > 0)
            {
                query = query.Where(
                    u => !searchCriteria.ExceptUserIDs.Contains(u.IMObjID));
            }

            if (searchCriteria.SubdivisionID.HasValue)
            {
                query = query.Where(u => Subdivision.SubdivisionIsSibling(searchCriteria.SubdivisionID, u.SubdivisionID));
            }

            if (searchCriteria.OrganizationID.HasValue)
            {
                query = query.Where(u => Group.UserInOrganizationItem(ObjectClass.Organizaton, searchCriteria.OrganizationID, u.IMObjID));
            }

            if (searchCriteria.ControlsObjectID.HasValue)
            {
                Expression<Func<User, bool>> hasCustomControl = user => _db
                    .Set<CustomControl>()
                        .Any(
                            x => x.UserId == user.IMObjID
                                    && x.ObjectId == searchCriteria.ControlsObjectID
                                    && x.ObjectClass == searchCriteria.ControlsObjectClassID);
                query = query.Where(
                    searchCriteria.ControlsObjectValue
                        ? hasCustomControl
                        : hasCustomControl.Not());
            }

            return query
                .Select(
                    u => new ObjectSearchResult
                    {
                        ID = u.IMObjID,
                        ClassID = ObjectClass.User,
                        FullName = User.GetFullName(u.IMObjID)
                    });
        }
    }
}
