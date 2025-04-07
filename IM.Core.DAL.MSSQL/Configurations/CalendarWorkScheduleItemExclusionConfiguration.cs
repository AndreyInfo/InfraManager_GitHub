using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class CalendarWorkScheduleItemExclusionConfiguration : CalendarWorkScheduleItemExclusionConfigurationBase
{
    protected override string KeyName => "PK_CalendarWorkScheduleItemExclusion";

    protected override string DFID => "NEWID()";

    protected override string ExclusionFK => "FK_CalendarWorkScheduleItemExclusion_Exclusion";

    protected override void ConfigureDatabase(EntityTypeBuilder<CalendarWorkScheduleItemExclusion> builder)
    {
        builder.ToTable("CalendarWorkScheduleItemExclusion", "dbo");

        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.CalendarWorkScheduleID).HasColumnName("CalendarWorkScheduleID");
        builder.Property(x => x.DayOfYear).HasColumnName("DayOfYear");
        builder.Property(x => x.ExclusionID).HasColumnName("ExclusionID");
        builder.Property(x => x.TimeStart).HasColumnType("smalldatetime").HasColumnName("TimeStart");
        builder.Property(x => x.TimeSpanInMinutes).HasColumnName("TimeSpanInMinutes");
    }
}
