using InfraManager.DAL.FormBuilder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class FormBuilderFormTabConfigurationBase : IEntityTypeConfiguration<FormTab>
{
    protected abstract string PK { get; }
    protected abstract string FK_Fields { get; }
    
    public void Configure(EntityTypeBuilder<FormTab> builder)
    {
        builder.HasKey(x => x.ID).HasName(PK);

        builder.Property(x => x.Model).HasMaxLength(50);
        builder.Property(x => x.Identifier).HasMaxLength(50);
        builder.Property(x => x.Icon).HasMaxLength(100);
        builder.Property(x => x.Name).HasMaxLength(500);
        
        builder.HasMany(x => x.Fields) 
            .WithOne()
            .HasForeignKey(x => x.TabID).HasConstraintName(FK_Fields);
        
        ConfigureDatabase(builder);
    }
    
    protected abstract void ConfigureDatabase(EntityTypeBuilder<FormTab> builder);
}