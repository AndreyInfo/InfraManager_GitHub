using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class CalendarHolidayConfiguration : CalendarHolidayConfigurationBase
{
    protected override string KeyName => "pk_calendar_holiday";
    protected override string UIName => "ui_name_calendar_holiday";
    protected override string ForeignKeyCalendarHoliday => "fk_calendar_holiday_item_calendar_holiday";

    protected override void ConfigureDataBase(EntityTypeBuilder<CalendarHoliday> builder)
    {
        builder.ToTable("calendar_holiday", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.ComplementaryID).HasColumnName("complementary_id");
        builder.HasXminRowVersion(x => x.RowVersion);
    }
}