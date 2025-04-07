using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class CalendarWorkScheduleDefaultConfiguration : CalendarWorkScheduleDefaultConfigurationBase
{
    protected override string CalendarWeekendFK => "fk_calendar_work_schedule_default_calendar_weekend";

    protected override string CalendarHolidayFK => "fk_calendar_work_schedule_default_calendar_holiday";

    protected override string TimeZoneFK => "fk_calendar_work_schedule_default_time_zone";

    protected override string KeyName => "calendar_work_schedule_default_pkey";

    protected override void ConfigureDataBase(EntityTypeBuilder<CalendarWorkScheduleDefault> builder)
    {
        builder.ToTable("calendar_work_schedule_default", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id");
        builder.Property(x => x.CalendarHolidayID).HasColumnName("calendar_holiday_id");
        builder.Property(x => x.CalendarWeekendID).HasColumnName("calendar_weekend_id");
        builder.Property(x => x.TimeZoneID).HasColumnName("time_zone_id");
        builder.Property(x => x.TimeStart).HasColumnType("timestamp without time zone")
            .HasColumnName("time_start");
        builder.Property(x => x.TimeEnd).HasColumnType("timestamp without time zone")
            .HasColumnName("time_end");
        builder.Property(x => x.TimeSpanInMinutes).HasColumnName("time_span_in_minutes");
        builder.Property(x => x.DinnerTimeStart).HasColumnType("timestamp without time zone")
            .HasColumnName("dinner_time_start");
        builder.Property(x => x.DinnerTimeEnd).HasColumnType("timestamp without time zone")
            .HasColumnName("dinner_time_end");
        builder.Property(x => x.ExclusionTimeSpanInMinutes).HasColumnName("exclusion_time_span_in_minutes");
        builder.HasXminRowVersion(x => x.RowVersion);
    }
}