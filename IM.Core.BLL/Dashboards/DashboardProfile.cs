using AutoMapper;
using InfraManager.BLL.Dashboards.ForTable;
using InfraManager.DAL.Dashboards;

namespace InfraManager.BLL.Dashboards;

internal class DashboardProfile : Profile
{
    public DashboardProfile()
    {
        CreateMap<Dashboard, DashboardFullDetails>();
        CreateMap<Dashboard, DashboardDetails>();
        CreateMap<DashboardData, Dashboard>();
        CreateMap<DashboardFullData, DashboardData>();
        CreateMap<DashboardDetails, DashboardFullDetails>();
        CreateMap<Dashboard, DashboardsForTableDetails>();
    }
}
