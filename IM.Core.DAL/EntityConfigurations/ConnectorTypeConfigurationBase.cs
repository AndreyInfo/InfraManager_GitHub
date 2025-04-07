using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;
public abstract class ConnectorTypeConfigurationBase : IEntityTypeConfiguration<ConnectorType>
{
    protected abstract string PrimaryKey { get; }
    protected abstract string MediumForeignKey { get; }
    protected abstract string UIName { get; }

    public void Configure(EntityTypeBuilder<ConnectorType> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKey);

        builder.HasIndex(x => x.Name, UIName).IsUnique();

        builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Image).HasMaxLength(50);

        builder.HasOne(x => x.Medium)
            .WithMany()
            .HasForeignKey(x => x.MediumID)
            .HasConstraintName(MediumForeignKey);

        ConfigureDatabase(builder);
    }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<ConnectorType> builder);
}
