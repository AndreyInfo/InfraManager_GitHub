using InfraManager.DAL.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class InframanagerClassConfigurationBase : IEntityTypeConfiguration<InframanagerObjectClass>
{
    protected abstract void ConfigureDatabase(EntityTypeBuilder<InframanagerObjectClass> builder);
    
    public void Configure(EntityTypeBuilder<InframanagerObjectClass> builder)
    {
        builder.HasKey(x => x.ID);
        builder.Property(e => e.Name).HasMaxLength(100);
        
        builder.HasMany(c => c.Operations)
            .WithOne(x => x.Class)
            .HasForeignKey(c => c.ClassID);

        ConfigureDatabase(builder);
    }
}