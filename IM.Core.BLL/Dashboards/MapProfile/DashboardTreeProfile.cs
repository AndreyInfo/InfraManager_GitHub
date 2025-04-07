using AutoMapper;
using InfraManager.DAL.Dashboards;

namespace InfraManager.BLL.Dashboards.MapProfile;

public class DashboardTreeProfile : Profile
{
    public DashboardTreeProfile()
    {
        CreateMap<DashboardTreeResultItem, DashboardTreeItemDetails>()
            .ForMember(m => m.ParentID, o => o.MapFrom(f => f.ParentFolderID));
    }
}