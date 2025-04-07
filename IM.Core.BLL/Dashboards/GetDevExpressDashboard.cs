using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.Dashboards;

namespace InfraManager.BLL.Dashboards;

internal sealed class GetDevExpressDashboard : IGetDashboard, ISelfRegisteredService<IGetDashboard>
{
    private readonly IDevExpressDashboardQuery _dashboardDevExpress;

    public GetDevExpressDashboard(
        IDevExpressDashboardQuery dashboardDevExpress
        )
    {
        _dashboardDevExpress = dashboardDevExpress;
    }

    public async Task<string> GetAsync(Guid dashboardID, CancellationToken cancellationToken = default)
    {
        return await _dashboardDevExpress.ExecuteAsync(dashboardID, cancellationToken);
    }
}