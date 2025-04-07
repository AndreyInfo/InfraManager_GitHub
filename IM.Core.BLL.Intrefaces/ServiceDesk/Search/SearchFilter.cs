using System;
using InfraManager.Services.SearchService;

namespace InfraManager.BLL.ServiceDesk.Search;

public class SearchFilter
{
    public int Take { get; init; }
    public int Skip { get; init; }
    public ObjectClass[] Classes { get; init; }
    public string Text { get; set; }
    public bool SearchFinished { get; init; }
    public SearchHelper.SearchMode SearchMode { get; init; }
    public string OrderBy { get; init; } = nameof(MyTasksReportItem.ID);
    public bool Ascending { get; init; }
    public Guid[]? IDs { get; set; }
}