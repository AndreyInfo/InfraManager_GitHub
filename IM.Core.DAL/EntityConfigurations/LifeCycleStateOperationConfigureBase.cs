using InfraManager.DAL.ProductCatalogue.LifeCycles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;
public abstract class LifeCycleStateOperationConfigureBase : IEntityTypeConfiguration<LifeCycleStateOperation>
{
    protected abstract string PrimaryKey { get; }
    protected abstract string LifeCycleStateFK { get; }
    protected abstract string WorkOrderTemplateFK { get; }
    protected abstract string RoleLifeCycleStateOperationsFK { get; }
    protected abstract string LifeCycleStateIX { get; }
    protected abstract string WorkOrderTemplateIX { get; }
    protected abstract string StateAndTemplateIX { get; }
    protected abstract string UINameByLifeCycleStateID { get; }

    public void Configure(EntityTypeBuilder<LifeCycleStateOperation> builder)
    {
        builder.HasKey(c => c.ID).HasName(PrimaryKey);

        builder.HasIndex(e => e.LifeCycleStateID, LifeCycleStateIX);
        builder.HasIndex(e => e.WorkOrderTemplateID, WorkOrderTemplateIX);
        builder.HasIndex(e => new { e.LifeCycleStateID, e.WorkOrderTemplateID }, StateAndTemplateIX);
        builder.HasIndex(e => new { e.Name, e.LifeCycleStateID }, UINameByLifeCycleStateID);


        builder.Property(e => e.LifeCycleStateID).ValueGeneratedOnAdd();
        builder.Property(e => e.Name).HasMaxLength(250).IsRequired(true);
        builder.Property(e => e.IconName).HasMaxLength(200).IsRequired(false);


        builder.HasOne(c => c.LifeCycleState)
            .WithMany(c => c.LifeCycleStateOperations)
            .HasForeignKey(c => c.LifeCycleStateID)
            .HasConstraintName(LifeCycleStateFK);

        builder.HasMany(c => c.RoleLifeCycleStateOperations)
            .WithOne(c => c.LifeCycleStateOperation)
            .HasForeignKey(c => c.LifeCycleStateOperationID)
            .HasConstraintName(RoleLifeCycleStateOperationsFK);

        builder.HasOne(c => c.WorkOrderTemplate)
            .WithMany()
            .HasForeignKey(c => c.WorkOrderTemplateID)
            .HasConstraintName(WorkOrderTemplateFK);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<LifeCycleStateOperation> builder);
}
