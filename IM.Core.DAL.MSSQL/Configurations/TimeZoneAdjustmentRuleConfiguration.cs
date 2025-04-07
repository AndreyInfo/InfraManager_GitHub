using Core = InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class TimeZoneAdjustmentRuleConfiguration : Core.TimeZoneAdjustmentRuleConfiguration
    {
        protected override string TableName => "TimeZoneAdjustmentRule";

        protected override string TableSchema => "dbo";

        protected override void ConfigureDbProvider(EntityTypeBuilder<TimeZoneAdjustmentRule> builder)
        {
            builder.Property(x => x.TimeZoneID).HasColumnName("TimeZoneID").IsRequired();
            builder.Property(x => x.DateStart).HasColumnName("DateStart").IsRequired();
            builder.Property(x => x.DateEnd).HasColumnName("DateEnd").IsRequired();
            builder.Property(x => x.DaylightDeltaInMinutes).HasColumnName("DaylightDeltaInMinutes").IsRequired();
            builder.Property(x => x.TransitionStart_IsFixedDateRule).HasColumnName("TransitionStart_IsFixedDateRule").IsRequired();
            builder.Property(x => x.TransitionStart_Month).HasColumnName("TransitionStart_Month").IsRequired();
            builder.Property(x => x.TransitionStart_Day).HasColumnName("TransitionStart_Day");
            builder.Property(x => x.TransitionStart_TimeOfDay).HasColumnName("TransitionStart_TimeOfDay").IsRequired();
            builder.Property(x => x.TransitionStart_Week).HasColumnName("TransitionStart_Week");
            builder.Property(x => x.TransitionStart_DayOfWeek).HasColumnName("TransitionStart_DayOfWeek");
            builder.Property(x => x.TransitionEnd_IsFixedDateRule).HasColumnName("TransitionEnd_IsFixedDateRule").IsRequired();
            builder.Property(x => x.TransitionEnd_Month).HasColumnName("TransitionEnd_Month").IsRequired();
            builder.Property(x => x.TransitionEnd_Day).HasColumnName("TransitionEnd_Day");
            builder.Property(x => x.TransitionEnd_TimeOfDay).HasColumnName("TransitionEnd_TimeOfDay").IsRequired();
            builder.Property(x => x.TransitionEnd_Week).HasColumnName("TransitionEnd_Week");
            builder.Property(x => x.TransitionEnd_DayOfWeek).HasColumnName("TransitionEnd_DayOfWeek");

        }
    }
}
