using IM.Core.DAL.Postgres;
using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

internal sealed class ConnectorTypeConfiguration : ConnectorTypeConfigurationBase
{
    protected override string PrimaryKey => "pk_connector_kinds";

    protected override string MediumForeignKey => "fk_connector_type_medium";

    protected override string UIName => "ui_name_connector_kinds";

    protected override void ConfigureDatabase(EntityTypeBuilder<ConnectorType> builder)
    {
        builder.ToTable("connector_kinds", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("identificator");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.PairCount).HasColumnName("pair_count");
        builder.Property(x => x.MediumID).HasColumnName("medium_id");
        builder.Property(x => x.Image).HasColumnName("image");
        builder.Property(x => x.ComplementaryID).HasColumnName("complementary_id");
    }
}