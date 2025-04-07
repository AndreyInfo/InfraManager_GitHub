using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.MaintenanceWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations;

internal sealed class MaintenanceDependencyConfiguration : MaintenanceDependencyConfigurationBase
{
    protected override string PrimaryKeyName => "pk_maintenance_dependency";
    protected override string MaintenanceFK => "fk_maintenance_dependency_maintenance";

    protected override void ConfigureDataBase(EntityTypeBuilder<MaintenanceDependency> builder)
    {
        builder.ToTable("maintenance_dependency", Options.Scheme);

        builder.Property(x => x.MaintenanceID).HasColumnName("maintenance_id");
        builder.Property(x => x.ObjectID).HasColumnName("object_id");
        builder.Property(x => x.ObjectName).HasColumnName("object_name");
        builder.Property(x => x.ObjectLocation).HasColumnName("object_location");
        builder.Property(x => x.ObjectClassID).HasColumnName("object_class_id");
        builder.Property(x => x.Note).HasColumnName("note");
        builder.Property(x => x.Type).HasColumnName("type");
        builder.Property(x => x.Locked).HasColumnName("locked");
    }
}