using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InfraManager.DAL.ServiceDesk.Problems
{
    internal class ProblemCauseSearchQuery : 
        IProblemCauseSearchQuery,
        ISelfRegisteredService<IProblemCauseSearchQuery>
    {
        private readonly DbSet<ProblemCause> _db;

        public ProblemCauseSearchQuery(DbSet<ProblemCause> db)
        {
            _db = db;
        }

        public IQueryable<ObjectSearchResult> Query(SearchCriteria searchCriteria)
        {
            var query = _db.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchCriteria.Text))
            {
                var searchPattern = string.Concat("%", searchCriteria.Text.ToStartsWithPattern());
                query = query.Where(x => EF.Functions.Like(x.Name, searchPattern));
            }

            return query.Select(
                x => new ObjectSearchResult
                {
                    ID = x.ID,
                    ClassID = ObjectClass.ProblemCause,
                    FullName = x.Name
                });
        }
    }
}
