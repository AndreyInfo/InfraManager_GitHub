using AutoMapper;
using InfraManager.DAL.Dashboards;

namespace InfraManager.BLL.Dashboards;

internal class DashboardFolderProfile : Profile
{
    public DashboardFolderProfile()
    {
        CreateMap<DashboardFolder, DashboardFolderDetails>();
        CreateMap<DashboardFolderData, DashboardFolder>();
        CreateMap<DashboardFolder, DashboardFolderWithParents>();
    }
}
