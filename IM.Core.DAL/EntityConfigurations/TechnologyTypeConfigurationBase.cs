using InfraManager.DAL.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;
public abstract class TechnologyTypeConfigurationBase : IEntityTypeConfiguration<TechnologyType>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string DefaultValueID { get; }
    protected abstract string UIName { get; }

    public void Configure(EntityTypeBuilder<TechnologyType> entity)
    {
        entity.HasKey(e => e.ID).HasName(PrimaryKeyName);

        entity.HasIndex(c=> c.Name, UIName).IsUnique();

        entity.Property(f => f.ID).ValueGeneratedOnAdd().HasDefaultValueSql(DefaultValueID);
        entity.Property(e => e.Name).HasMaxLength(50).IsRequired(false);

        ConfigureDataBase(entity);
    }
    
    protected abstract void ConfigureDataBase(EntityTypeBuilder<TechnologyType> entity);
}
