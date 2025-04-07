using AutoMapper;
using InfraManager.DAL.MaintenanceWork;
using System.Linq;

namespace InfraManager.BLL.MaintenanceWork;
internal sealed class MaintenanceWorkProfile : Profile
{
    public MaintenanceWorkProfile()
    {
        CreateMap<MaintenanceNodeTree, MaintenanceNodeTreeDetails>();
        
        CreateMap<Maintenance, MaintenanceNodeTreeDetails>()
            .ForMember(dst => dst.ClassID, m => m.MapFrom(scr => ObjectClass.Maintenance))
            .ForMember(dst => dst.ParentID, m => m.MapFrom(scr => scr.FolderID));

        CreateMap<MaintenanceFolder, MaintenanceNodeTreeDetails>()
            .ForMember(dst => dst.ClassID, m => m.MapFrom(scr => ObjectClass.MaintenanceFolder))
            .ForMember(dst => dst.HasChild, m => m.MapFrom(scr => scr.SubFolders.Any() || scr.Maintenances.Any()));
    }
}
