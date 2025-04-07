using Inframanager.BLL.ListView;
using InfraManager.BLL.DataList;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk
{
    internal class MyTaskBLL : IMyTaskBLL, ISelfRegisteredService<IMyTaskBLL>
    {
        private readonly IListViewBLL<MyTasksReportItem, ServiceDeskListFilter> _dataListBuilder;

        public MyTaskBLL(IListViewBLL<MyTasksReportItem, ServiceDeskListFilter> dataListBuilder)
        {
            _dataListBuilder = dataListBuilder;
        }

        public Task<MyTasksReportItem[]> GetListAsync(
            ListViewFilterData<ServiceDeskListFilter> filterBy,
            CancellationToken cancellationToken = default)
        {
            return _dataListBuilder.BuildAsync(filterBy, cancellationToken);
        }
    }
}