using InfraManager.DAL.CalendarWorkSchedules;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

public sealed class CalendarWorkScheduleShiftExclusionConfiguration : CalendarWorkScheduleShiftExclusionConfigurationBase
{
    protected override string KeyName => "PK_CalendarWorkScheduleShiftExclusion";

    protected override string DefaultValueID => "(newid())";

    protected override string CalendarWorkScheduleShiftFK => "FK_CalendarWorkScheduleShiftExclusion_CalendarWorkScheduleShift";

    protected override string ExclusionFK => "FK_CalendarWorkScheduleShiftExclusion_Exclusion";

    protected override void ConfigureDataBase(EntityTypeBuilder<CalendarWorkScheduleShiftExclusion> builder)
    {
        builder.ToTable("CalendarWorkScheduleShiftExclusion", "dbo");

        builder.Property(e => e.ID).HasColumnName("ID");
        builder.Property(e => e.CalendarWorkScheduleShiftID).HasColumnName("CalendarWorkScheduleShiftID");
        builder.Property(e => e.ExclusionID).HasColumnName("ExclusionID");
        builder.Property(e => e.TimeStart).HasColumnName("TimeStart");
        builder.Property(e => e.TimeSpanInMinutes).HasColumnName("TimeSpanInMinutes");
    }
}

