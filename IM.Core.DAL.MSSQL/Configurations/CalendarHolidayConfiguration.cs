using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class CalendarHolidayConfiguration : CalendarHolidayConfigurationBase
{
    protected override string KeyName => "PK_CalendarHoliday";
    protected override string UIName => "UI_CalendarHoliday_Name";
    protected override string ForeignKeyCalendarHoliday => "FK_CalendarHolidayItem_CalendarHoliday";

    protected override void ConfigureDataBase(EntityTypeBuilder<CalendarHoliday> builder)
    {
        builder.ToTable("CalendarHoliday", "dbo");

        builder.Property(c => c.ID).HasColumnName("ID");
        builder.Property(c => c.Name).HasColumnName("Name");
        builder.Property(c => c.RowVersion).IsRowVersion().HasColumnName("RowVersion");  
        builder.Property(c => c.ComplementaryID).HasColumnName("ComplementaryID");  
    }
}
