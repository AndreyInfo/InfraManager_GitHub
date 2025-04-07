using AutoMapper;
using InfraManager.BLL.Parameters;
using InfraManager.DAL.Parameters;

namespace InfraManager.BLL.Mapping
{
    internal class ParameterEnumMap : Profile
    {
        public ParameterEnumMap()
        {
            CreateMap<ParameterEnum, ParameterEnumDetails>().ReverseMap();
        }
    }
}
