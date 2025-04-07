using InfraManager.DAL.MaintenanceWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class MaintenanceDependencyConfigurationBase : IEntityTypeConfiguration<MaintenanceDependency>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string MaintenanceFK { get; }

    public void Configure(EntityTypeBuilder<MaintenanceDependency> builder)
    {
        builder.HasKey(e => new { e.MaintenanceID, e.ObjectID }).HasName(PrimaryKeyName);

        builder.Property(c => c.Note).HasMaxLength(1000).IsRequired(true);
        builder.Property(c => c.ObjectName).HasMaxLength(1000).IsRequired(true);
        builder.Property(c => c.ObjectLocation).HasMaxLength(2000).IsRequired(true);

        builder.HasOne(x => x.Maintenance)
            .WithMany(p => p.MaintenanceDependencies)
            .HasForeignKey(x => x.MaintenanceID)
            .HasConstraintName(MaintenanceFK);

        builder.HasOne(x => x.ObjectClass)
           .WithMany(p => p.MaintenanceDependencies)
           .HasForeignKey(x => x.ObjectClassID);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<MaintenanceDependency> builder);
}
