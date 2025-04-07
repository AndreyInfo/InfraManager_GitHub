using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class CalendarWorkScheduleItemConfiguration : CalendarWorkScheduleItemConfigurationBase
{
    protected override string PrimaryKeyName => "PK_CalendarWorkScheduleItem";

    protected override string CalendarWorkScheduleFK => "FK_CalendarWorkScheduleItem_CalendarWorkSchedule";

    protected override void ConfigureDataBase(EntityTypeBuilder<CalendarWorkScheduleItem> builder)
    {
        builder.ToTable("CalendarWorkScheduleItem", "dbo");

        builder.Property(x => x.CalendarWorkScheduleID).HasColumnName("CalendarWorkScheduleID");
        builder.Property(x => x.DayOfYear).HasColumnName("DayOfYear");
        builder.Property(x => x.TimeStart).HasColumnType("datetime2(7)").HasColumnName("TimeStart");
        builder.Property(x => x.TimeSpanInMinutes).HasColumnName("TimeSpanInMinutes");
        builder.Property(x => x.ShiftNumber).HasColumnName("ShiftNumber");
        builder.Property(x => x.DayType).HasColumnName("DayType");
    }
}

