using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.Asset
{
    public class TerminalDeviceProfile : Profile
    {
        public TerminalDeviceProfile()
        {
            CreateMap<TerminalDevice, TerminalDeviceDetails>();
        }
    }
}
