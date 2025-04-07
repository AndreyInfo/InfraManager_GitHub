using Inframanager.BLL.Settings.TableFilters;
using InfraManager.DAL.Settings;
using System;

namespace InfraManager.BLL
{
    public class ListFilterBase : ITableFilter
    {
        public int StartRecordIndex { get; init; }
        public int CountRecords { get; init; }
        public string ViewName { get; init; }
        public int TimezoneOffsetInMinutes { get; init; }
        public Guid? CurrentFilterID { get; init; }
        public string StandardFilter { get; init; }
        public FilterElementData[] CustomFilters { get; init; } = Array.Empty<FilterElementData>();
    }
}
