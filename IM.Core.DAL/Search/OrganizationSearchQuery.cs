using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace InfraManager.DAL.Search
{
    internal class OrganizationSearchQuery : IOrganizationSearchQuery,
        ISelfRegisteredService<IOrganizationSearchQuery>
    {
        private readonly DbSet<Organization> _organizations;

        public OrganizationSearchQuery(DbSet<Organization> organizations)
        {
            _organizations = organizations;
        }

        public IQueryable<ObjectSearchResult> Query(
            OrganizationSearchCriteria searchBy, 
            Guid? currentUserId = default)
        {
            IQueryable<Organization> query = _organizations.AsNoTracking();

            if (currentUserId.HasValue)
            {
                query = query
                    .Where(
                        x => DbFunctions.AccessIsGranted(
                            ObjectClass.Organizaton,
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
                        x => x.Subdivisions
                            .SelectMany(s => s.Users)
                            .Any(u => u.IMObjID == searchBy.UserId));
            }

            if (!string.IsNullOrWhiteSpace(searchBy.Text))
            {
                var searchPattern = $"%{searchBy.Text.ToStartsWithPattern()}";
                query = query.Where(
                    x => EF.Functions.Like(x.Name, searchPattern)
                        || EF.Functions.Like(x.Note, searchPattern));
            }

            return query.Select(
                x => new ObjectSearchResult
                {
                    ClassID = ObjectClass.Organizaton, 
                    FullName = x.Name, 
                    ID = x.ID
                }); 
        }
    }
}
