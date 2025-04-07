using AutoMapper;
using InfraManager.DAL.Software;

namespace InfraManager.BLL.Software.SoftwareModelDependencies;

public class SoftwareModelDependencyProfile : Profile
{
    public SoftwareModelDependencyProfile()
    {
        CreateMap<SoftwareModelDependencyData, SoftwareModelDependency>();
        CreateMap<SoftwareModelDependency, SoftwareModelDependencyDetails>();

    }
}
