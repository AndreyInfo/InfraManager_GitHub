using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class CallReferenceConfiguration : CallReferenceConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_CallReference";

        protected override void ConfigureDatabase(EntityTypeBuilder<CallReference> builder)
        {
            builder.ToTable("CallReference", "dbo");
            builder.Property(x => x.CallID).HasColumnName("CallID");
            builder.Property(x => x.ObjectID).HasColumnName("ObjectID");
            builder.Property(x => x.ObjectClassID).HasColumnName("ClassID");
        }
    }
}
