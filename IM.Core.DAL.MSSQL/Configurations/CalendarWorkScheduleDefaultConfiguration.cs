using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class CalendarWorkScheduleDefaultConfiguration : CalendarWorkScheduleDefaultConfigurationBase
{
    protected override string CalendarWeekendFK => "FK_CalendarWorkScheduleDefault_CalendarWeekend";

    protected override string CalendarHolidayFK => "FK_CalendarWorkScheduleDefault_CalendarHoliday";

    protected override string TimeZoneFK => "FK_CalendarWorkScheduleDefault_TimeZone";

    protected override string KeyName => "PK_CalendarWorkScheduleDefault";

    protected override void ConfigureDataBase(EntityTypeBuilder<CalendarWorkScheduleDefault> builder)
    {
        builder.ToTable("CalendarWorkScheduleDefault", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.CalendarHolidayID).HasColumnName("CalendarHolidayID");
        builder.Property(x => x.CalendarWeekendID).HasColumnName("CalendarWeekendID");
        builder.Property(x => x.TimeZoneID).HasColumnName("TimeZoneID");
        builder.Property(x => x.TimeStart).HasColumnType("smalldatetime").HasColumnName("TimeStart");
        builder.Property(x => x.TimeEnd).HasColumnType("smalldatetime").HasColumnName("TimeEnd");
        builder.Property(x => x.TimeSpanInMinutes).HasColumnName("TimeSpanInMinutes");
        builder.Property(x => x.ExclusionTimeSpanInMinutes).HasColumnName("ExclusionTimeSpanInMinutes");
        builder.Property(x => x.DinnerTimeStart).HasColumnType("smalldatetime").HasColumnName("DinnerTimeStart");
        builder.Property(x => x.DinnerTimeEnd).HasColumnType("smalldatetime").HasColumnName("DinnerTimeEnd");
        builder.Property(x => x.RowVersion).IsRowVersion()
            .HasColumnName("RowVersion")
            .HasColumnType("timestamp");
    }
}
