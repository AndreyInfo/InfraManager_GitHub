using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal class CalendarWeekendConfiguration : CalendarWeekendConfigurationBase
{
    protected override string KeyName => "PK_CalendarWeekend";

    protected override string UniqueIndexByName => "UI_CalendarWeekend_Name";

    protected override void ConfigureDataBase(EntityTypeBuilder<CalendarWeekend> builder)
    {
        builder.ToTable("CalendarWeekend", "dbo");

        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.Name).HasColumnName("Name");
        builder.Property(x => x.Sunday).HasColumnName("Sunday");
        builder.Property(x => x.Monday).HasColumnName("Monday");
        builder.Property(x => x.Tuesday).HasColumnName("Tuesday");
        builder.Property(x => x.Wednesday).HasColumnName("Wednesday");
        builder.Property(x => x.Thursday).HasColumnName("Thursday");
        builder.Property(x => x.Friday).HasColumnName("Friday");
        builder.Property(x => x.Saturday).HasColumnName("Saturday");
        builder.Property(x => x.ComplementaryID).HasColumnName("ComplementaryID");
        builder.Property(x => x.RowVersion).HasColumnName("RowVersion").HasColumnType("timestamp").IsRowVersion();
    }
}
