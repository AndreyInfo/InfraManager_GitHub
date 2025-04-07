using InfraManager.DAL.AccessManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class OperationConfigurationBase : IEntityTypeConfiguration<Operation>
{
    protected abstract string KeyName { get; }
    public void Configure(EntityTypeBuilder<Operation> builder)
    {
        builder.HasKey(x => x.ID).HasName(KeyName);

        builder.Property(x => x.ClassID).IsRequired(true);
        builder.Property(x => x.Name).HasMaxLength(255).IsRequired(true);
        builder.Property(x => x.OperationName).HasMaxLength(100).IsRequired(true);
        builder.Property(x => x.Description).HasMaxLength(350).IsRequired(false);

        // TODO �������� FK
        builder.HasMany(c => c.RoleOperations)
            .WithOne(c => c.Operation)
            .HasForeignKey(c => c.OperationID)
            .OnDelete(DeleteBehavior.Cascade);

        // todo: Нужно избавиться от использования InframanagerObjectClass
        builder.HasOne(c => c.Class)
            .WithMany(c => c.Operations)
            .HasForeignKey(c => c.ClassID)
            .HasPrincipalKey(x => x.ID);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<Operation> builder);
}