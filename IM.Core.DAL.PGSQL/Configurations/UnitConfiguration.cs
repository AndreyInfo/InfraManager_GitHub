using IM.Core.DAL.Postgres;
using Inframanager.DAL.EntityConfigurations;
using Inframanager.DAL.ProductCatalogue.Units;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations;

internal class UnitConfiguration : UnitConfigurationBase
{
    protected override string PrimaryKeyName => "pk_unit";

    protected override string UIName => "ui_name_unit";

    protected override void ConfigureDatabase(EntityTypeBuilder<Unit> builder)
    {
        builder.ToTable("unit", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("unit_id");

        builder.Property(x => x.Name).HasColumnName("name");

        builder.Property(x => x.ComplementaryID).HasColumnName("complementary_id");

        builder.Property(x => x.Code).HasColumnName("code");

        builder.Property(x => x.ExternalID).HasColumnName("external_id");
    }
}