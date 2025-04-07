using AutoMapper;
using InfraManager.BLL.Asset;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.Mapping;

public class MediumProfile : Profile
{
    public MediumProfile()
    {
        CreateMap<Medium, MediumDetails>();
    }
}
