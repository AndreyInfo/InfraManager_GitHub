using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class ChangeRequestDependencyConfiguration : ChangeRequestDependencyConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_CallDependency";

        protected override void ConfigureDatabase(
            EntityTypeBuilder<ChangeRequestDependency> builder)
        {
            builder.ToTable("RFCDependency", "dbo");
            builder.Property(x => x.Locked).HasColumnName("Locked");
            builder.Property(x => x.Note).HasColumnName("Note");
            builder.Property(x => x.ObjectClassID).HasColumnName("ObjectClassID");
            builder.Property(x => x.ObjectID).HasColumnName("ObjectID");
            builder.Property(x => x.ObjectLocation).HasColumnName("ObjectLocation");
            builder.Property(x => x.ObjectName).HasColumnName("ObjectName");
            builder.Property(x => x.OwnerObjectID).HasColumnName("RFCID");
            builder.Property(x => x.Type).HasColumnName("Type");
        }
    }
}
