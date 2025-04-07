using AutoMapper;
using InfraManager.BLL.Parameters;
using InfraManager.DAL.Parameters;
using System.Linq;

namespace InfraManager.BLL.Mapping
{
    internal class ParameterEnumValueMap : Profile
    {
        public ParameterEnumValueMap()
        {
            CreateMap<ParameterEnumValue, ParameterEnumValueData>()
                .ForMember(dst => dst.HasChild,
                 m => m.MapFrom(scr => scr.ParameterEnums.Any()
                 ? true : false))
                .ReverseMap();
        }
    }
}