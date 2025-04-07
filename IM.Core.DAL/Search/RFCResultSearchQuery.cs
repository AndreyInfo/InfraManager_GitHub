using System.Linq;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.Search;

public class RFCResultSearchQuery: IRFCResultSearchQuery, ISelfRegisteredService<IRFCResultSearchQuery>
{
    private readonly DbSet<RequestForServiceResult> _rfcResults;

    public RFCResultSearchQuery(DbSet<RequestForServiceResult> changeRequestResults) => _rfcResults = changeRequestResults;
    public IQueryable<ObjectSearchResult> Query(SearchCriteria searchBy)
    {
        var rfcResultQuery = _rfcResults.AsNoTracking()
            .Where(rfcResult => !rfcResult.Removed);

        if (!string.IsNullOrWhiteSpace(searchBy.Text))
        {
            // EF.Functions.Like - регистрозависим в PostgreSQL, регистронезависим в MS SQL
            // поэтому принудительно сравниваем строки в нижнем регистре
            var searchPattern = searchBy.Text.Trim().ToLower().ToContainsPattern();
            rfcResultQuery =
                rfcResultQuery.Where(rfcResult => EF.Functions.Like(rfcResult.Name.ToLower(), searchPattern));
        }

        return rfcResultQuery.Select(rfcResult => new ObjectSearchResult
        {
            ID = rfcResult.ID,
            FullName = rfcResult.Name,
            ClassID = ObjectClass.RFCResult
        });
    }
}