using AutoMapper;
using InfraManager.BLL.Parameters;
using InfraManager.DAL.Parameters;
using System.Linq;

namespace InfraManager.BLL.Mapping
{
    internal class ParameterEnumValuesDataProfile : Profile
    {
        public ParameterEnumValuesDataProfile()
        {
                CreateMap<ParameterEnumValue, ParameterEnumValuesData>()
                    .ForMember(dst => dst.Parent, m => m.MapFrom(scr => scr))
                    .ForMember(dst => dst.Childrens, opt => { opt.Condition(p => p.ParameterEnum.IsTree); opt.MapFrom(scr => scr.ParameterEnums); })
                    .ReverseMap();
            
        }
    }
}