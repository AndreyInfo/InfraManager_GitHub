using AutoMapper;
using ActivePortEntity = InfraManager.DAL.Asset.ActivePort;

namespace InfraManager.BLL.Asset.ActivePort;

internal class ActivePortProfile : Profile
{
    public ActivePortProfile()
    {
        CreateMap<ActivePortEntity, ActivePortDetails>();
        CreateMap<ActivePortData, ActivePortEntity>();
    }
}