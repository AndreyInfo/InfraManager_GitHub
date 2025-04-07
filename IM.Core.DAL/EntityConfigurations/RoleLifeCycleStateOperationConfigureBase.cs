using InfraManager.DAL.AccessManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;
public abstract class RoleLifeCycleStateOperationConfigureBase : IEntityTypeConfiguration<RoleLifeCycleStateOperation>
{
    protected abstract string PrimaryKey { get; }
    protected abstract string RoleFK { get; }
    protected abstract string LifeCycleOperationFK { get; }
    public void Configure(EntityTypeBuilder<RoleLifeCycleStateOperation> builder)
    {
        builder.HasKey(c => new { c.RoleID, c.LifeCycleStateOperationID }).HasName(PrimaryKey);

        builder.HasOne(c => c.LifeCycleStateOperation)
            .WithMany(c => c.RoleLifeCycleStateOperations)
            .HasForeignKey(c => c.LifeCycleStateOperationID)
            .HasConstraintName(LifeCycleOperationFK)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Role)
            .WithMany(c => c.LifeCycleStateOperations)
            .HasForeignKey(c => c.RoleID)
            .HasConstraintName(RoleFK)
            .OnDelete(DeleteBehavior.Cascade);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<RoleLifeCycleStateOperation> builder);
}
