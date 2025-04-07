using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;
public abstract class MediumConfigurationBase : IEntityTypeConfiguration<Medium>
{
    protected abstract string PrimaryKey { get; }

    public void Configure(EntityTypeBuilder<Medium> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKey);

        builder.Property(x => x.Name).IsRequired().HasMaxLength(50);

        builder.HasMany(x => x.ConnectorTypes)
            .WithOne(x => x.Medium)
            .HasForeignKey(x => x.MediumID);

        ConfigureDatabase(builder);
    }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<Medium> builder);
}
