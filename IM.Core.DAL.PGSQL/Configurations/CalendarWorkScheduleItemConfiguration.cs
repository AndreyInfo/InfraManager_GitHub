using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class CalendarWorkScheduleItemConfiguration : CalendarWorkScheduleItemConfigurationBase
{
    protected override string PrimaryKeyName => "pk_calendar_work_schedule_item";

    protected override string CalendarWorkScheduleFK => "fk_calendar_work_schedule_item_calendar_work_schedule";

    protected override void ConfigureDataBase(EntityTypeBuilder<CalendarWorkScheduleItem> builder)
    {
        builder.ToTable("calendar_work_schedule_item", Options.Scheme);

        builder.Property(x => x.CalendarWorkScheduleID).HasColumnName("calendar_work_schedule_id");
        builder.Property(x => x.DayOfYear).HasColumnName("day_of_year");
        builder.Property(x => x.TimeStart).HasColumnType("timestamp without time zone").HasColumnName("time_start");
        builder.Property(x => x.TimeSpanInMinutes).HasColumnName("time_span_in_minutes");
        builder.Property(x => x.ShiftNumber).HasColumnName("shift_number");
        builder.Property(x => x.DayType).HasColumnName("day_type");
    }
}