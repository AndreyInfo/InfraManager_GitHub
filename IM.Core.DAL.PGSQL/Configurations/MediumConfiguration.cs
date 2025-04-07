using IM.Core.DAL.Postgres;
using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

internal sealed class MediumConfiguration : MediumConfigurationBase
{
    protected override string PrimaryKey => "pk_transmission_media_kinds";

    protected override void ConfigureDatabase(EntityTypeBuilder<Medium> builder)
    {
        builder.ToTable("transmission_media_kinds", Options.Scheme);

        builder.Property(e => e.ID).HasColumnName("identificator");
        builder.Property(e => e.Name).HasColumnName("name");
    }
}