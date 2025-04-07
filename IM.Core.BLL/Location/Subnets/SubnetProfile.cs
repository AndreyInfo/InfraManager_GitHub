using AutoMapper;
using InfraManager.DAL.Location;

namespace InfraManager.BLL.Location.Subnets;

internal class SubnetProfile : Profile
{
    public SubnetProfile()
    {
        CreateMap<BuildingSubnet, SubnetDetails>()
            .ReverseMap();
    }
}
