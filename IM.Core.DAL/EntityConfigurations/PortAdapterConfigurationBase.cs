using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class PortAdapterConfigurationBase : IEntityTypeConfiguration<PortAdapter>
{
    protected abstract string PrimaryKeyName { get; }

    protected abstract string Schema { get; }

    protected abstract string TableName { get; }

    protected abstract void AdditionalConfig(EntityTypeBuilder<PortAdapter> entity);

    public void Configure(EntityTypeBuilder<PortAdapter> entity)
    {
        entity.ToTable(TableName, Schema);
        entity.Property(e => e.ID);
        entity.Property(e => e.ObjectID);
        entity.Property(e => e.PortNumber);
        entity.Property(e => e.JackTypeID);
        entity.Property(e => e.TechnologyID);
        entity.Property(e => e.PortAddress);
        entity.Property(e => e.Note);

        entity.HasKey(x => x.ID).HasName(PrimaryKeyName);

        AdditionalConfig(entity);
    }
}
