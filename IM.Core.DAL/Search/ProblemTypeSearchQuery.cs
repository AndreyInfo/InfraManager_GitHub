using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InfraManager.DAL.Search
{
    internal class ProblemTypeSearchQuery : IProblemTypeSearchQuery,
        ISelfRegisteredService<IProblemTypeSearchQuery>
    {
        private readonly DbSet<ProblemType> _problemTypes;

        public ProblemTypeSearchQuery(DbSet<ProblemType> problemTypes)
        {
            _problemTypes = problemTypes;
        }

        public IQueryable<ObjectSearchResult> Query(string text)
        {
            IQueryable<ProblemType> query = _problemTypes.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(text))
            {
                var searchPattern = $"%{text.ToStartsWithPattern()}";
                query = query
                    .Where(x => 
                        EF.Functions.Like(
                            ProblemType.GetFullProblemTypeName(x.ID), searchPattern));
            }

            return query
                .Select(
                    x => new ObjectSearchResult
                    {
                        ID = x.ID,
                        ClassID = ObjectClass.ProblemType,
                        FullName = ProblemType.GetFullProblemTypeName(x.ID)
                    });
        }
    }
}
