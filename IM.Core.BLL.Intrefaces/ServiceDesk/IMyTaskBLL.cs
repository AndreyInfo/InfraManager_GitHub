using Inframanager.BLL.ListView;
using InfraManager.BLL.DataList;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk
{
    public interface IMyTaskBLL
    {
        Task<MyTasksReportItem[]> GetListAsync(
            ListViewFilterData<ServiceDeskListFilter> filterBy,
            CancellationToken cancellationToken = default);
    }
}