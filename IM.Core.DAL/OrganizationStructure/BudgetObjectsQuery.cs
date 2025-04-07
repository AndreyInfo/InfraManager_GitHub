using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InfraManager.DAL.OrganizationStructure
{
    internal class BudgetObjectsQuery : IBudgetObjectsQuery, ISelfRegisteredService<IBudgetObjectsQuery>
    {
        private readonly DbSet<Organization> _organizations;
        private readonly DbSet<Subdivision> _subdivisions;

        public BudgetObjectsQuery(DbSet<Organization> organizations, DbSet<Subdivision> subdivisions)
        {
            _organizations = organizations;
            _subdivisions = subdivisions;
        }

        public IQueryable<BudgetObject> Query()
        {
            return _organizations.AsNoTracking()
                .Select(
                    x => 
                        new BudgetObject 
                        { 
                            ID = x.ID, 
                            Name = x.Name, 
                            ClassID = ObjectClass.Organizaton 
                        })
                .Union(
                    _subdivisions.AsNoTracking()
                        .Select(
                            x => 
                                new BudgetObject
                                {
                                    ID = x.ID,
                                    Name = x.Name + " \\ " + Subdivision.GetFullSubdivisionName(x.ID),
                                    ClassID = ObjectClass.Division
                                }));
        }
    }
}
