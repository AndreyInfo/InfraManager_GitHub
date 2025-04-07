using AutoMapper;
using InfraManager.DAL.Snmp;

namespace InfraManager.BLL.Snmp
{
    public class SnmpDeviceModelProfile : Profile
    {
        public SnmpDeviceModelProfile()
        {
            CreateMap<SnmpDeviceModel, SnmpDeviceModelDetails>();
            CreateMap<SnmpDeviceModelData, SnmpDeviceModel>();
        }
    }
}
