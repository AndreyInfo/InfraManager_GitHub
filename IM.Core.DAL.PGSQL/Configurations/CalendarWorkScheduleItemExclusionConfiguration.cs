using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class CalendarWorkScheduleItemExclusionConfiguration : CalendarWorkScheduleItemExclusionConfigurationBase
{
    protected override string KeyName => "pk_calendar_work_schedule_item_exclusion";

    protected override string DFID => "gen_random_uuid()";

    protected override string ExclusionFK => "fk_calendar_work_schedule_item_exclusion_exclusion";

    protected override void ConfigureDatabase(EntityTypeBuilder<CalendarWorkScheduleItemExclusion> builder)
    {
        builder.ToTable("calendar_work_schedule_item_exclusion", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id");
        builder.Property(x => x.CalendarWorkScheduleID).HasColumnName("calendar_work_schedule_id");
        builder.Property(x => x.DayOfYear).HasColumnName("day_of_year");
        builder.Property(x => x.ExclusionID).HasColumnName("exclusion_id");
        builder.Property(x => x.TimeStart).HasColumnType("timestamp without time zone").HasColumnName("time_start");
        builder.Property(x => x.TimeSpanInMinutes).HasColumnName("time_span_in_minutes");
    }
}