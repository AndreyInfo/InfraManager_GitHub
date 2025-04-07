using Inframanager.DAL.ProductCatalogue.Units;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inframanager.DAL.EntityConfigurations;

public abstract class UnitConfigurationBase : IEntityTypeConfiguration<Unit>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string UIName { get; }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<Unit> entity);

    public void Configure(EntityTypeBuilder<Unit> entity)
    {
        entity.HasKey(x => x.ID).HasName(PrimaryKeyName);

        entity.Property(x => x.Name).IsRequired(false).HasMaxLength(50);

        entity.Property(x => x.Code).IsRequired(false).HasMaxLength(50);

        entity.Property(x => x.ExternalID).IsRequired(false).HasMaxLength(250);


        ConfigureDatabase(entity);
    }
}