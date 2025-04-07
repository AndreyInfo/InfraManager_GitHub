using InfraManager.DAL.CalendarWorkSchedules;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class CalendarWorkScheduleShiftConfiguration : CalendarWorkScheduleShiftConfigurationBase
{
    protected override string PrimaryKey => "PK_CalendarWorkScheduleShift";

    protected override string CalendarWorkcScheduleNumberUI => "UI_CalendarWorkSchedule_Number_Shift";

    protected override string DefaultValueID => "NEWID()";

    protected override string CalendarWorkScheduleFK => "FK_CalendarWorkScheduleShift_CalendarWorkSchedule";

    protected override void ConfigureDataBase(EntityTypeBuilder<CalendarWorkScheduleShift> builder)
    {
        builder.ToTable("CalendarWorkScheduleShift", "dbo");

        builder.Property(e => e.ID).HasColumnName("ID");
        builder.Property(c => c.CalendarWorkScheduleID).HasColumnName("CalendarWorkScheduleID");
        builder.Property(c => c.Number).HasColumnName("Number");
        builder.Property(c => c.TimeStart).HasColumnName("TimeStart");
        builder.Property(c => c.TimeSpanInMinutes).HasColumnName("TimeSpanInMinutes");
    }
}
