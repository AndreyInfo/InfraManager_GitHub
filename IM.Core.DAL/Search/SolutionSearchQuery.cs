using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.WorkOrders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.DAL.Search
{
    internal class SolutionSearchQuery: ISolutionSearchQuery, ISelfRegisteredService<ISolutionSearchQuery>
    {
        private readonly DbSet<Solution> _solutions;

        public SolutionSearchQuery(DbSet<Solution> solutions)
        {
            _solutions= solutions;
        }

        public IQueryable<ObjectSearchResult> Query(SolutionByNameCriteria criteria)
        {
            var searchQuery = _solutions.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(criteria?.Text))
            {
                var searchStr = criteria.Text.ToContainsPattern();
                searchQuery = searchQuery.Where(x => EF.Functions.Like(x.Name.ToLower(), searchStr) || EF.Functions.Like(x.Description.ToLower(), searchStr));
            }
            return searchQuery.Select( x => new ObjectSearchResult
                    {
                        ID = x.ID,
                        ClassID = ObjectClass.Solution,
                        FullName = x.Name,
                        Details = x.Description                        
                    });
        }
    }
}
