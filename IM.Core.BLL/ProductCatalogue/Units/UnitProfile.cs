using AutoMapper;
using Inframanager.DAL.ProductCatalogue.Units;

namespace InfraManager.BLL.ProductCatalogue.Units;

internal class UnitProfile : Profile
{
    public UnitProfile()
    {
        CreateMap<Unit, UnitDetails>();

        CreateMap<UnitData, Unit>();
    }
}