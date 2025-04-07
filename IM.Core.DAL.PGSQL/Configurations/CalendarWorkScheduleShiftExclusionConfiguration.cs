using IM.Core.DAL.Postgres;
using InfraManager.DAL.CalendarWorkSchedules;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

public sealed class CalendarWorkScheduleShiftExclusionConfiguration : CalendarWorkScheduleShiftExclusionConfigurationBase
{
    protected override string KeyName => "pk_calendar_work_schedule_shift_exclusion";

    protected override string DefaultValueID => "(gen_random_uuid())";

    protected override string CalendarWorkScheduleShiftFK => "fk_calendar_work_schedule_shift_exclusion_calendar_work_schedul";

    protected override string ExclusionFK => "fk_calendar_work_schedule_shift_exclusion_exclusion";

    protected override void ConfigureDataBase(EntityTypeBuilder<CalendarWorkScheduleShiftExclusion> builder)
    {
        builder.ToTable("calendar_work_schedule_shift_exclusion", Options.Scheme);

        builder.Property(e => e.ID).HasDefaultValueSql("(gen_random_uuid())").HasColumnName("id");
        builder.Property(c => c.CalendarWorkScheduleShiftID).HasColumnName("calendar_work_schedule_shift_id");
        builder.Property(c => c.ExclusionID).HasColumnName("exclusion_id");
        builder.Property(c => c.TimeStart).HasColumnName("time_start");
        builder.Property(c => c.TimeSpanInMinutes).HasColumnName("time_span_in_minutes");
    }
}

