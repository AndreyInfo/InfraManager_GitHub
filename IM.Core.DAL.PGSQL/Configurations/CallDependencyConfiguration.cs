using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class CallDependencyConfiguration : CallDependencyConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_problem_dependency";

        protected override void ConfigureDatabase(
            EntityTypeBuilder<CallDependency> builder)
        {
            builder.ToTable("call_dependency", Options.Scheme);
            builder.Property(x => x.Locked).HasColumnName("locked");
            builder.Property(x => x.Note).HasColumnName("note");
            builder.Property(x => x.ObjectClassID).HasColumnName("object_class_id");
            builder.Property(x => x.ObjectID).HasColumnName("object_id");
            builder.Property(x => x.ObjectLocation).HasColumnName("object_location");
            builder.Property(x => x.ObjectName).HasColumnName("object_name");
            builder.Property(x => x.OwnerObjectID).HasColumnName("call_id");
            builder.Property(x => x.Type).HasColumnName("type");
        }
    }
}
