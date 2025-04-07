using AutoMapper;
using InfraManager.DAL.MaintenanceWork;

namespace InfraManager.BLL.MaintenanceWork.MaintenanceDependencies;

public sealed class MaintenanceDependencyProfile : Profile
{
    public MaintenanceDependencyProfile()
    {
        CreateMap<MaintenanceDependency, MaintenanceDependencyDetails>()
            .ForMember(dst => dst.ClassName, m => m.MapFrom(scr => scr.ObjectClass.Name));

        CreateMap<MaintenanceDependencyData, MaintenanceDependency>()
            .ConstructUsing(c=> new MaintenanceDependency(c.MaintenanceID, c.ObjectID));
    }
}
