using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class ManufacturerConfigurationBase : IEntityTypeConfiguration<Manufacturer>
{
    protected abstract string UiName { get; }
    protected abstract string ImObjIDIndexName { get; }
    protected abstract string PrimaryKeyName { get; }
    protected abstract string DefaultValueID { get; }
    protected abstract string DefaultValueImObjID { get; }

    public void Configure(EntityTypeBuilder<Manufacturer> entity)
    {
        entity.HasKey(e => e.ID).HasName(PrimaryKeyName);

        entity.HasIndex(e => e.Name, UiName).IsUnique();
        entity.HasIndex(e => e.ImObjID, ImObjIDIndexName).IsUnique();

        entity.Property(c=> c.ID).ValueGeneratedOnAdd().HasDefaultValueSql(DefaultValueID);
        entity.Property(c=> c.ImObjID).HasDefaultValueSql(DefaultValueImObjID);
        entity.Property(x => x.Name)
            .IsRequired(true)
            .HasMaxLength(255);

        entity.Property(e => e.ExternalID)
            .IsRequired(true)
            .HasMaxLength(250);

        ConfigureDatabase(entity);
    }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<Manufacturer> entity);

}