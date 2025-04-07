using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal class CalendarHolidayItemConfiguration : CalendarHolidayItemConfigurationBase
{
    protected override string KeyName => "pk_calendar_holiday_item";

    protected override void ConfigureDataBase(EntityTypeBuilder<CalendarHolidayItem> builder)
    {
        builder.ToTable("calendar_holiday_item", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id");
        builder.Property(x => x.CalendarHolidayID).HasColumnName("calendar_holiday_id");
        builder.Property(x => x.Day).HasColumnName("day");
        builder.Property(x => x.Month).HasColumnName("month");
    }
}