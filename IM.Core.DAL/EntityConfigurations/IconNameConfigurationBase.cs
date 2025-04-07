using InfraManager.DAL.ProductCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class IconNameConfigurationBase: IEntityTypeConfiguration<Icon>
{
    protected abstract string PrimaryKeyName { get; }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<Icon> builder);
    
    public void Configure(EntityTypeBuilder<Icon> builder)
    {
        
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);
        builder.Property(x => x.ID);
        builder.Property(x => x.Name).HasMaxLength(50);
        builder.HasIndex(x => x.Name).IsUnique();
        
        ConfigureDatabase(builder);
    }
}