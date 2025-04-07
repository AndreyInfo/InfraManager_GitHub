using IM.Core.DAL.Postgres;
using Core = InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class TimeZoneAdjustmentRuleConfiguration : Core.TimeZoneAdjustmentRuleConfiguration
    {
        protected override string TableName => "time_zone_adjustment_rule";

        protected override string TableSchema => Options.Scheme;

        protected override void ConfigureDbProvider(EntityTypeBuilder<TimeZoneAdjustmentRule> builder)
        {
            builder.Property(x => x.TimeZoneID).HasColumnName("time_zone_id").IsRequired();
            builder.Property(x => x.DateStart).HasColumnName("date_start").IsRequired();
            builder.Property(x => x.DateEnd).HasColumnName("date_end").IsRequired();
            builder.Property(x => x.DaylightDeltaInMinutes).HasColumnName("daylight_delta_in_minutes").IsRequired();
            builder.Property(x => x.TransitionStart_IsFixedDateRule)
                .HasColumnName("transition_start_is_fixed_date_rule").IsRequired();
            builder.Property(x => x.TransitionStart_Month).HasColumnName("transition_start_month").IsRequired();
            builder.Property(x => x.TransitionStart_Day).HasColumnName("transition_start_day");
            builder.Property(x => x.TransitionStart_TimeOfDay).HasColumnName("transition_start_time_of_day")
                .IsRequired();
            builder.Property(x => x.TransitionStart_Week).HasColumnName("transition_start_week");
            builder.Property(x => x.TransitionStart_DayOfWeek).HasColumnName("transition_start_day_of_week");
            builder.Property(x => x.TransitionEnd_IsFixedDateRule).HasColumnName("transition_end_is_fixed_date_rule")
                .IsRequired();
            builder.Property(x => x.TransitionEnd_Month).HasColumnName("transition_end_month").IsRequired();
            builder.Property(x => x.TransitionEnd_Day).HasColumnName("transition_end_day");
            builder.Property(x => x.TransitionEnd_TimeOfDay).HasColumnName("transition_end_time_of_day").IsRequired();
            builder.Property(x => x.TransitionEnd_Week).HasColumnName("transition_end_week");
            builder.Property(x => x.TransitionEnd_DayOfWeek).HasColumnName("transition_end_day_of_week");
        }
    }
}