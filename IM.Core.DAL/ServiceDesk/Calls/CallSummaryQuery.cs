using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk.Calls;

internal class CallSummaryQuery : ICallSummaryQuery
    , ISelfRegisteredService<ICallSummaryQuery>
{
    private readonly IPagingQueryCreator _pagging;
    public CallSummaryQuery(IPagingQueryCreator pagging)
    {
        _pagging = pagging;
    }

    public async Task<CallSummaryModelItem[]> ExecuteAsync(IExecutableQuery<CallSummary> query, PaggingFilter filter, Sort orderColumn, string[] mappedValues, CancellationToken cancellationToken)
    {
        var selectQuery = query.Select(c => new CallSummaryModelItem
        {
            ID = c.ID,
            Name = c.Name,
            ServiceItemID = c.ServiceItemID,
            ServiceAttendanceID = c.ServiceAttendanceID,
            Visible = c.Visible,
            RowVersion = c.RowVersion,

            ServiceItemName = c.ServiceItem.Name,
            ServiceAttendanceName = c.ServiceAttendance.Name,

            ServiceName = c.ServiceItem.Service.Name
                                            ?? c.ServiceAttendance.Service.Name,
            ServiceCategoryName = c.ServiceItem.Service.Category.Name
                                            ?? c.ServiceAttendance.Service.Category.Name,

            ItemOrAttendanceName = c.ServiceItem.Name ?? c.ServiceAttendance.Name,

        });

        // Придумать как переиспользовать сортировки
        var orderedQuery = selectQuery.OrderBy(orderColumn);

        for (int i = 1; i < mappedValues.Length; i++)
        {
            var orderedColumn = orderColumn with { PropertyName = mappedValues[i] };
            orderedQuery = orderedQuery.ThenOrderBy(orderedColumn);
        }


        var paggingQuery = _pagging.Create(orderedQuery);
        return await paggingQuery.PageAsync(filter.Skip, filter.Take, cancellationToken);
    }
}
