using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal class CalendarWeekendConfiguration : CalendarWeekendConfigurationBase
{
    protected override string KeyName => "pk_calendar_weekend";

    protected override string UniqueIndexByName => "ui_calendar_weekend_name";

    protected override void ConfigureDataBase(EntityTypeBuilder<CalendarWeekend> builder)
    {
        builder.ToTable("calendar_weekend", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.Sunday).HasColumnName("sunday");
        builder.Property(x => x.Monday).HasColumnName("monday");
        builder.Property(x => x.Tuesday).HasColumnName("tuesday");
        builder.Property(x => x.Wednesday).HasColumnName("wednesday");
        builder.Property(x => x.Thursday).HasColumnName("thursday");
        builder.Property(x => x.Friday).HasColumnName("friday");
        builder.Property(x => x.Saturday).HasColumnName("saturday");
        builder.Property(x => x.ComplementaryID).HasColumnName("complementary_id");
        builder.HasXminRowVersion(x => x.RowVersion);
    }
}