using InfraManager.DAL.ProductCatalogue.LifeCycles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;
public abstract class LifeCycleStateOperationTransitionConfigurationBase : IEntityTypeConfiguration<LifeCycleStateOperationTransition>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string StateFK { get; }
    protected abstract string OperationFK { get; }
    protected abstract string OperationIX { get; }
    public void Configure(EntityTypeBuilder<LifeCycleStateOperationTransition> builder)
    {
        builder.HasKey(c => c.ID).HasName(PrimaryKeyName);

        builder.HasIndex(c=> c.OperationID, OperationIX);

        builder.Property(c=> c.OperationID).ValueGeneratedOnAdd();

        builder.HasOne(c => c.FinishState)
            .WithMany()
            .HasForeignKey(c => c.FinishStateID)
            .HasConstraintName(StateFK)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Operation)
            .WithMany(c => c.Transitions)
            .HasForeignKey(c => c.OperationID)
            .HasConstraintName(OperationFK)
            .OnDelete(DeleteBehavior.Cascade);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<LifeCycleStateOperationTransition> builder);
}
