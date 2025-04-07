using Inframanager.BLL.ListView;
using InfraManager.BLL;
using InfraManager.BLL.ServiceDesk;
using InfraManager.BLL.ServiceDesk.WorkOrders;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk
{
    public static class ListFilterExtensions
    {
        public static ListViewFilterData<ServiceDeskListFilter> ToServiceDeskFilter(
            this ListFilter filterBy)
        {

            return new ListViewFilterData<ServiceDeskListFilter>
            {
                ExtensionFilter = new ServiceDeskListFilter
                {
                    WithFinishedWorkflow = filterBy.WithFinishedWorkflow,
                    AfterModifiedMilliseconds = filterBy.AfterModifiedMilliseconds,
                    IDList = filterBy.IDList
                },
                CurrentFilterID = filterBy.CurrentFilterID,
                CustomFilters = filterBy.CustomFilters,
                StandardFilter = filterBy.StandardFilter,
                ViewName = filterBy.ViewName,
                Take = filterBy.CountRecords,
                Skip = filterBy.StartRecordIndex
            };
        }
        
        public static ListViewFilterData<WorkOrderListFilter> ToWorkOrderServiceDeskFilter(
                    this WorkOrderListFilter filterBy, BaseFilter baseFilter)
                {
                    return new ListViewFilterData<WorkOrderListFilter>
                    {
                        ExtensionFilter = filterBy,
                        ViewName = baseFilter.ViewName,
                        Take = baseFilter.CountRecords,
                        Skip = baseFilter.StartRecordIndex
                    };
                }

        public static ListViewFilterData<CallFromMeListFilter> ToCallFromMeListFilter(
           this ListFilter filterBy)
        {

            return new ListViewFilterData<CallFromMeListFilter>
            {
                ExtensionFilter = new CallFromMeListFilter
                {
                    WithFinishedWorkflow = filterBy.WithFinishedWorkflow,
                    AfterModifiedMilliseconds = filterBy.AfterModifiedMilliseconds,
                    IDList = filterBy.IDList
                },
                CurrentFilterID = filterBy.CurrentFilterID,
                CustomFilters = filterBy.CustomFilters,
                StandardFilter = filterBy.StandardFilter,
                ViewName = filterBy.ViewName,
                Take = filterBy.CountRecords,
                Skip = filterBy.StartRecordIndex
            };
        }
    }
}
