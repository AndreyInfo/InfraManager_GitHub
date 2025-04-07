using System;

namespace InfraManager.BLL.ServiceDesk.Search
{
    public class SearchServiceStatus
    {
        public DateTime? LastIndexRebuild { get; init; }
        public SearchServiceTask CurrentTask { get; init; }
    }

    public enum SearchServiceTask
    {
        Idle = 0,
        IndexRebuilding,
        IndexOptimizing
    }
}