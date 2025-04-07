using AutoMapper;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.Location.TimeZones;

public class TimeZoneProfile : Profile
{
    public TimeZoneProfile()
    {
        CreateMap<TimeZone, TimeZoneDetails>()
            .ForMember(dst => dst.AdjustmentRules,
                mapper => mapper.MapFrom(
                    src => src.TimeZoneAdjustmentRules));


        CreateMap<TimeZoneAdjustmentRule, TimeZoneAdjustmentRuleDetails>()

            .ForPath(dst => dst.TransitionStart.IsFixedDateRule,
                mapper => mapper.MapFrom(
                    src => src.TransitionStart_IsFixedDateRule))
            .ForPath(dst => dst.TransitionStart.Month,
                mapper => mapper.MapFrom(
                    src => src.TransitionStart_Month))
            .ForPath(dst => dst.TransitionStart.Day,
                mapper => mapper.MapFrom(
                    src => src.TransitionStart_Day))
            .ForPath(dst => dst.TransitionStart.TimeOfDay,
                mapper => mapper.MapFrom(
                    src => src.TransitionStart_TimeOfDay))
            .ForPath(dst => dst.TransitionStart.Week,
                mapper => mapper.MapFrom(
                    src => src.TransitionStart_Week))
            .ForPath(dst => dst.TransitionStart.DayOfWeek,
                mapper => mapper.MapFrom(
                    src => src.TransitionStart_DayOfWeek))

            .ForPath(dst => dst.TransitionEnd.IsFixedDateRule,
                mapper => mapper.MapFrom(
                    src => src.TransitionEnd_IsFixedDateRule))
            .ForPath(dst => dst.TransitionEnd.Month,
                mapper => mapper.MapFrom(
                    src => src.TransitionEnd_Month))
            .ForPath(dst => dst.TransitionEnd.Day,
                mapper => mapper.MapFrom(
                    src => src.TransitionEnd_Day))
            .ForPath(dst => dst.TransitionEnd.TimeOfDay,
                mapper => mapper.MapFrom(
                    src => src.TransitionEnd_TimeOfDay))
            .ForPath(dst => dst.TransitionEnd.Week,
                mapper => mapper.MapFrom(
                    src => src.TransitionEnd_Week))
            .ForPath(dst => dst.TransitionEnd.DayOfWeek,
                mapper => mapper.MapFrom(
                    src => src.TransitionEnd_DayOfWeek))
            ;
    }
}