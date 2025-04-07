using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class SolutionConfigurationBase : IEntityTypeConfiguration<Solution>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string UIName { get; }

    public void Configure(EntityTypeBuilder<Solution> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

        builder.HasIndex(c=> c.Name, UIName).IsUnique();

        builder.Property(x => x.ID).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired(true).HasMaxLength(250);
        builder.Property(x => x.Description).IsRequired(true);
        builder.Property(x => x.HTMLDescription).IsRequired(true);

        ConfigureDatabase(builder);
    }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<Solution> builder);

}
