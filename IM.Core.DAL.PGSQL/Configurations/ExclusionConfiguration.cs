using IM.Core.DAL.Postgres;
using InfraManager.DAL.Calendar;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace InfraManager.DAL.Postgres.Configurations;

internal class ExclusionConfiguration : ExclusionConfigurationBase
{
    protected override string KeyName => "pk_exclusion";

    protected override string FKCalendarExclusionsName => "fk_calendar_exclusion_exclusion";

    protected override string DefaultValueSQL => "gen_random_uuid()";

    protected override string UIName => "ui_name_exclusion";

    protected override void ConfigureDataBase(EntityTypeBuilder<Exclusion> builder)
    {
        builder.ToTable("exclusion", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.Type).HasColumnName("type");
        builder.HasXminRowVersion(e => e.RowVersion);
    }
}