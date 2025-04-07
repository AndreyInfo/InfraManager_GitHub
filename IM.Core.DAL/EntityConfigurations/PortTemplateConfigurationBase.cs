using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class PortTemplateConfigurationBase:IEntityTypeConfiguration<PortTemplate>
{
    protected abstract string PrimaryKeyName { get; }
    
    protected abstract string Schema { get; }
    
    protected abstract string TableName { get; }
    
    protected abstract void AdditionalConfig(EntityTypeBuilder<PortTemplate> entity);
    
    public void Configure(EntityTypeBuilder<PortTemplate> entity)
    {
        entity.ToTable(TableName, Schema);
        entity.Property(e => e.ObjectID);
        entity.Property(e => e.ClassID);
        entity.Property(e => e.PortNumber);
        entity.Property(e => e.JackTypeID);
        entity.Property(e => e.TechnologyID);

        entity.HasKey(e => new {ID = e.ObjectID, e.PortNumber}).HasName(PrimaryKeyName);
        
        AdditionalConfig(entity);

    }
}