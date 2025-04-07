using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.MaintenanceWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations;

internal sealed class MaintenanceDependencyConfiguration : MaintenanceDependencyConfigurationBase
{
    protected override string PrimaryKeyName => "PK_MaintenanceDependency";
    protected override string MaintenanceFK => "FK_MaintenanceDependency_Maintenancee";

    protected override void ConfigureDataBase(EntityTypeBuilder<MaintenanceDependency> builder)
    {
        builder.ToTable("MaintenanceDependency", Options.Scheme);

        builder.Property(x => x.MaintenanceID).HasColumnName("MaintenanceID");
        builder.Property(x => x.ObjectID).HasColumnName("ObjectID");
        builder.Property(x => x.ObjectName).HasColumnName("ObjectName");
        builder.Property(x => x.ObjectLocation).HasColumnName("ObjectLocation");
        builder.Property(x => x.ObjectClassID).HasColumnName("ObjectClassID");
        builder.Property(x => x.Note).HasColumnName("Note");
        builder.Property(x => x.Type).HasColumnName("Type");
        builder.Property(x => x.Locked).HasColumnName("Locked");
    }
}
