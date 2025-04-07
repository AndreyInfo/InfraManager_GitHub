using AutoMapper;
using InfraManager.DAL.Calendar;

namespace InfraManager.BLL.Calendar.CalendarExclusions
{
    public class CalendarExclusionProfile : Profile
    {
        public CalendarExclusionProfile()
        {
            CreateMap<CalendarExclusion, CalendarExclusionDetails>()
                .ForMember(dst => dst.ExclusionName, m => m.MapFrom(scr => scr.Exclusion.Name))
                .ForMember(dst => dst.Durability, m => m.MapFrom(scr => scr.UtcPeriodEnd.Subtract(scr.UtcPeriodStart).Minutes))
                .ForMember(dst => dst.RelatedObjectID, m => m.MapFrom(scr => scr.ObjectID))
                .ForMember(dst => dst.RelatedObjectClassID, m => m.MapFrom(scr => scr.ObjectClassID))
                .ReverseMap()
                .ForMember(x => x.Exclusion, y => y.Ignore())
                .ForMember(x => x.ServiceReference, y => y.Ignore())
                ;

            CreateMap<CalendarExclusionInsertDetails, CalendarExclusion>()
                .ConstructUsing(c => new(c.ObjectClassID, c.ObjectID
                                         , c.ExclusionID
                                         , c.UtcPeriodStart, c.UtcPeriodEnd
                                         , c.IsWorkPeriod
                                         , c.ServiceClassID, c.ServiceID));
        }
    }
}
