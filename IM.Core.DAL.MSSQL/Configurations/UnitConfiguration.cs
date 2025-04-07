using IM.Core.DAL.Microsoft.SqlServer;
using Inframanager.DAL.EntityConfigurations;
using Inframanager.DAL.ProductCatalogue.Units;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations;

internal class UnitConfiguration : UnitConfigurationBase
{
    protected override string PrimaryKeyName => "PK_Unit";

    protected override string UIName => "UI_Name_Unit";

    protected override void ConfigureDatabase(EntityTypeBuilder<Unit> builder)
    {
        builder.ToTable("Unit", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("UnitID");

        builder.Property(x => x.Name).HasColumnName("Name");

        builder.Property(x => x.ComplementaryID).HasColumnName("ComplementaryID");

        builder.Property(x => x.Code).HasColumnName("Code");

        builder.Property(x => x.ExternalID).HasColumnName("ExternalID");
    }
}