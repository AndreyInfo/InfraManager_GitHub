using AutoMapper;

namespace InfraManager.BLL.Asset.LifeCycleCommands.DeviceLocation;
internal sealed class DeviceLocationProfile : Profile
{
    public DeviceLocationProfile()
    {
        CreateMap<LifeCycleCommandBaseData, DeviceLocationData>();
        CreateMap<DeviceLocationDetails, LifeCycleCommandResultItem>();
    }
}
