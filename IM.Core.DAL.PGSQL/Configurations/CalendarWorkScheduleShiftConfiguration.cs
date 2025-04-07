using IM.Core.DAL.Postgres;
using InfraManager.DAL.CalendarWorkSchedules;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations;

internal sealed class CalendarWorkScheduleShiftConfiguration : CalendarWorkScheduleShiftConfigurationBase
{
    protected override string PrimaryKey => "pk_calendar_work_schedule_shift";

    protected override string CalendarWorkcScheduleNumberUI => "ui_calendar_work_schedule_number_shift";

    protected override string DefaultValueID => "(gen_random_uuid())";

    protected override string CalendarWorkScheduleFK => "fk_calendar_work_schedule_shift_calendar_work_schedule";

    protected override void ConfigureDataBase(EntityTypeBuilder<CalendarWorkScheduleShift> builder)
    {
        builder.ToTable("calendar_work_schedule_shift", Options.Scheme);

        builder.Property(e => e.ID).HasColumnName("id");
        builder.Property(c => c.CalendarWorkScheduleID).HasColumnName("calendar_work_schedule_id");
        builder.Property(c => c.Number).HasColumnName("number");
        builder.Property(c => c.TimeStart).HasColumnName("time_start");
        builder.Property(c => c.TimeSpanInMinutes).HasColumnName("time_span_in_minutes");
    }
}