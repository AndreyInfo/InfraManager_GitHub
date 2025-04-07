using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace InfraManager.DAL.Search
{
    internal class SubdivisionSearchQuery : ISubdivisionSearchQuery,
        ISelfRegisteredService<ISubdivisionSearchQuery>
    {
        private readonly DbSet<Subdivision> _subdivisions;

        public SubdivisionSearchQuery(DbSet<Subdivision> subdivisions)
        {
            _subdivisions = subdivisions;
        }

        public IQueryable<ObjectSearchResult> Query(
            SubdivisionSearchCriteria searchBy,
            Guid? currentUserId = default)
        {
            IQueryable<Subdivision> query = _subdivisions.AsNoTracking();
            
            if (currentUserId.HasValue)
            {
                query = query
                    .Where(
                        x => DbFunctions.AccessIsGranted(
                            ObjectClass.Division,
                            x.ID,
                            currentUserId.Value,
                            ObjectClass.User,
                            AccessManagement.AccessTypes.TOZ_org,
                            false));
            }

            if (searchBy.UserId.HasValue)
            {
                query = query
                    .Where(
                        x => Subdivision.SubdivisionIsSibling(x.Users.Single(u => u.IMObjID == searchBy.UserId).SubdivisionID, x.ID));
            }

            if (!string.IsNullOrWhiteSpace(searchBy.Text))
            {
                var searchPattern = searchBy.Text.ToContainsPattern();
                query = query.Where(
                    x => EF.Functions.Like(x.Name, searchPattern)
                        || EF.Functions.Like(x.Note, searchPattern));
            }

            return query.Select(
                x => new ObjectSearchResult
                {
                    ClassID = ObjectClass.Division,
                    FullName = x.Name,
                    ID = x.ID
                });
        }
    }
}
