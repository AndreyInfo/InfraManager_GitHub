using InfraManager.DAL.AccessManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class RoleConfigurationBase : IEntityTypeConfiguration<Role>
{
    protected abstract string KeyName { get; }
    protected abstract string DefaultValueID { get; }
    protected abstract string LifeCycleOperationFK { get; }
    protected abstract string UI_Name { get; }
    
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(x => x.ID).HasName(KeyName);

        builder.Property(x => x.ID)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql(DefaultValueID);

        builder.Property(x => x.Name)
            .IsRequired(true)
            .HasMaxLength(250);

        builder.Property(x => x.Note)
            .IsRequired(true)
            .HasMaxLength(500);

        builder.HasIndex(x => x.Name, UI_Name).IsUnique();

        //TODO добавить FK
        builder.HasMany(c => c.Operations)
            .WithOne()
            .HasForeignKey(c => c.RoleID);

        builder.HasMany(c => c.LifeCycleStateOperations)
            .WithOne(c => c.Role)
            .HasForeignKey(c => c.RoleID)
            .HasConstraintName(LifeCycleOperationFK);
        
        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<Role> builder);
}
