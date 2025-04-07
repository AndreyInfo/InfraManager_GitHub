using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.DAL.Calendar;

namespace InfraManager.BLL.Calendar.Exclusions;

internal class ExclusionProfile : Profile
{
    public ExclusionProfile()
    {
        CreateMap<Exclusion, ExclusionDetails>()
            .ForMember(dst => dst.TypeName, m => m.MapFrom<LocalizedEnumResolver<Exclusion, ExclusionDetails, ExclusionType>,
                        ExclusionType>(
                        entity => entity.Type))
            .ReverseMap()
            .ForMember(dst => dst.CalendarExclusions, m => m.Ignore());
    }
}
