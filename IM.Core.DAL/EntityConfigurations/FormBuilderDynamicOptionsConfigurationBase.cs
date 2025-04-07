using InfraManager.DAL.FormBuilder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class FormBuilderDynamicOptionsConfigurationBase : IEntityTypeConfiguration<DynamicOptions>
{
    protected abstract string PK { get; }
    
    public void Configure(EntityTypeBuilder<DynamicOptions> builder)
    {
        builder.HasKey(x => x.ID).HasName(PK);

        builder.Property(x => x.ParentIdentifier).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Constant).HasMaxLength(255);

        ConfigureDatabase(builder);
    }
    
    protected abstract void ConfigureDatabase(EntityTypeBuilder<DynamicOptions> builder);
}