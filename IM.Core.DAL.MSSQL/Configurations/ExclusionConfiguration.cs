using InfraManager.DAL.Calendar;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal class ExclusionConfiguration : ExclusionConfigurationBase
{
    protected override string KeyName => "PK_Exclusion";

    protected override string FKCalendarExclusionsName => "FK_CalendarExclusion_Exclusion";

    protected override string DefaultValueSQL => "NEWID()";

    protected override string UIName => "UI_Name_Exclusion";

    protected override void ConfigureDataBase(EntityTypeBuilder<Exclusion> builder)
    {
        builder.ToTable("Exclusion", "dbo");

        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.Name).HasColumnName("Name");
        builder.Property(x => x.Type).HasColumnName("Type");
        builder.Property(x => x.RowVersion).IsRowVersion().HasColumnName("RowVersion");
    }
}