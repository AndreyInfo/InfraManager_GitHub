using InfraManager.DAL.ProductCatalogue.LifeCycles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;
public abstract class LifeCycleStateConfigurationBase : IEntityTypeConfiguration<LifeCycleState>
{
    protected abstract string PrimaryKey { get; }
    protected abstract string DefaultValueID { get; }
    protected abstract string LifeCycleIX { get; }
    protected abstract string LifeCycleFK { get; }
    protected abstract string UINameByLifeCycleID { get; }

    public void Configure(EntityTypeBuilder<LifeCycleState> builder)
    {
        builder.HasKey(c => c.ID).HasName(PrimaryKey);

        builder.HasIndex(e => e.LifeCycleID, LifeCycleIX);
        builder.HasIndex(e => new { e.Name, e.LifeCycleID }, UINameByLifeCycleID).IsUnique();

        builder.Property(c => c.LifeCycleID).ValueGeneratedOnAdd();
        builder.Property(e => e.ID).HasDefaultValueSql(DefaultValueID);
        builder.Property(e => e.Name).HasMaxLength(250).IsRequired(true);

        builder.HasOne(c => c.LifeCycle)
            .WithMany(c => c.LifeCycleStates)
            .HasForeignKey(c => c.LifeCycleID)
            .HasConstraintName(LifeCycleFK);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<LifeCycleState> builder);
}
