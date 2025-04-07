using InfraManager.DAL.FormBuilder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class FormBuilderFormConfigurationBase : IEntityTypeConfiguration<Form>
{
    protected abstract string PK { get; }
    protected abstract string FK_FormTab { get; }
    
    public void Configure(EntityTypeBuilder<Form> builder)
    {
        builder.HasKey(x => x.ID).HasName(PK);
        builder.Property(x => x.Identifier).HasMaxLength(500);
        
        builder.HasMany(d => d.FormTabs)
            .WithOne()
            .HasForeignKey(d => d.FormID).HasConstraintName(FK_FormTab);

        ConfigureDatabase(builder);
    }
    
    protected abstract void ConfigureDatabase(EntityTypeBuilder<Form> builder);
}