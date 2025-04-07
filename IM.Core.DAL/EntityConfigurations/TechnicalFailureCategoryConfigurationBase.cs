using InfraManager.DAL.ServiceDesk.MassIncidents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class TechnicalFailureCategoryConfigurationBase : IEntityTypeConfiguration<TechnicalFailureCategory>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string UniqueKeyName { get; }
    protected abstract string IMObjIDUniqueKeyName { get; }
    
    public void Configure(EntityTypeBuilder<TechnicalFailureCategory> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);
        
        builder.Property(x => x.Name).HasMaxLength(100);
        builder.HasIndex(x => x.Name, UniqueKeyName).IsUnique();

        builder.Property(x => x.IMObjID).ValueGeneratedOnAdd();
        builder.HasIndex(x => x.IMObjID, IMObjIDUniqueKeyName).IsUnique();

        ConfigureDataProvider(builder);
    }
    
    protected abstract void ConfigureDataProvider(EntityTypeBuilder<TechnicalFailureCategory> builder);
}