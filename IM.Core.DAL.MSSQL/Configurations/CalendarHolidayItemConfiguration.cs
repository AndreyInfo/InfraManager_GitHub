using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal class CalendarHolidayItemConfiguration : CalendarHolidayItemConfigurationBase
{
    protected override string KeyName => "PK_CalendarHolidayItem";

    protected override void ConfigureDataBase(EntityTypeBuilder<CalendarHolidayItem> builder)
    {
        builder.ToTable("CalendarHolidayItem", "dbo");

        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.CalendarHolidayID).HasColumnName("CalendarHolidayID");
        builder.Property(x => x.Day).HasColumnName("Day");
        builder.Property(x => x.Month).HasColumnName("Month");
    }
}
