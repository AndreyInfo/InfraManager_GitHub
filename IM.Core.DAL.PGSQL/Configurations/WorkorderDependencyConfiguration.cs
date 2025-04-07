using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class WorkorderDependencyConfiguration : WorkorderDependencyConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_work_order_dependency";

        protected override void ConfigureDatabase(
            EntityTypeBuilder<WorkorderDependency> builder)
        {
            builder.ToTable("work_order_dependency", Options.Scheme);
            builder.Property(x => x.Locked).HasColumnName("locked");
            builder.Property(x => x.Note).HasColumnName("note");
            builder.Property(x => x.ObjectClassID).HasColumnName("object_class_id");
            builder.Property(x => x.ObjectID).HasColumnName("object_id");
            builder.Property(x => x.ObjectLocation).HasColumnName("object_location");
            builder.Property(x => x.ObjectName).HasColumnName("object_name");
            builder.Property(x => x.OwnerObjectID).HasColumnName("work_order_id");
            builder.Property(x => x.Type).HasColumnName("type");
        }
    }
}
