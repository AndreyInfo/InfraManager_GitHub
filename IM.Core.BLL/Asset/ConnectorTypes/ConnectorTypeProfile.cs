using AutoMapper;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.Asset.ConnectorTypes;
internal sealed class ConnectorTypeProfile : Profile
{
    public ConnectorTypeProfile()
    {
        CreateMap<ConnectorType, ConnectorTypeDetails>();
        CreateMap<ConnectorTypeData, ConnectorType>();
    }
}
