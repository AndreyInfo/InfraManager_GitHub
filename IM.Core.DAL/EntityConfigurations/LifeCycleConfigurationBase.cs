using InfraManager.DAL.ProductCatalogue.LifeCycles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;
public abstract class LifeCycleConfigurationBase : IEntityTypeConfiguration<LifeCycle>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string LifeCycleStateFK { get; }
    protected abstract string FormForeignKey { get; }
    protected abstract string DefaultValueID { get; }
    protected abstract string NameUI { get; }

    public void Configure(EntityTypeBuilder<LifeCycle> builder)
    {
        builder.HasKey(e => e.ID).HasName(PrimaryKeyName);

        builder.HasQueryFilter(LifeCycle.IsNotRemoved);

        builder.HasIndex(c => c.Name, NameUI).IsUnique();

        builder.Property(e => e.ID).HasDefaultValueSql(DefaultValueID);
        builder.Property(e => e.Name).HasMaxLength(250).IsRequired(true);

        builder.HasMany(c => c.LifeCycleStates)
            .WithOne(c => c.LifeCycle)
            .HasForeignKey(c => c.LifeCycleID)
            .HasConstraintName(LifeCycleStateFK);

        builder.HasOne(c => c.Form)
            .WithMany()
            .HasForeignKey(c => c.FormID)
            .HasConstraintName(FormForeignKey);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<LifeCycle> builder);
}
