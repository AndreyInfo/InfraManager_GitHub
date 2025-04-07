using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.ChangeRequests
{
    /// <summary>
    ///
    /// </summary>
    public interface IChangeRequestCategoryBLL
    {
        Task<ChangeRequestCategoryDetails[]> GetListAsync(CancellationToken cancellationToken = default);

    }
}
