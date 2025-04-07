using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ProductCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations;

public class IconNamesConfiguration : IconNameConfigurationBase
{
    protected override string PrimaryKeyName => "icon_names_pkey";

    protected override void ConfigureDatabase(EntityTypeBuilder<Icon> builder)
    {
        builder.ToTable("icons", Options.Scheme);
        builder.Property(x => x.ID).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
        builder.Property(x => x.Name).HasColumnName("name");
    }
}