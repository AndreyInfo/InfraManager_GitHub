using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class CallDependencyConfiguration : CallDependencyConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_CallDependency";

        protected override void ConfigureDatabase(
            EntityTypeBuilder<CallDependency> builder)
        {
            builder.ToTable("CallDependency", "dbo");
            builder.Property(x => x.Locked).HasColumnName("Locked");
            builder.Property(x => x.Note).HasColumnName("Note");
            builder.Property(x => x.ObjectClassID).HasColumnName("ObjectClassID");
            builder.Property(x => x.ObjectID).HasColumnName("ObjectID");
            builder.Property(x => x.ObjectLocation).HasColumnName("ObjectLocation");
            builder.Property(x => x.ObjectName).HasColumnName("ObjectName");
            builder.Property(x => x.OwnerObjectID).HasColumnName("CallID");
            builder.Property(x => x.Type).HasColumnName("Type");
        }
    }
}
