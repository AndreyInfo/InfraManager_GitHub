using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations
{
    internal class CallReferenceConfiguration : CallReferenceConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_call_reference";

        protected override void ConfigureDatabase(EntityTypeBuilder<CallReference> builder)
        {
            builder.ToTable("call_reference", Options.Scheme);
            builder.Property(x => x.CallID).HasColumnName("call_id");
            builder.Property(x => x.ObjectID).HasColumnName("object_id");
            builder.Property(x => x.ObjectClassID).HasColumnName("class_id");
        }
    }
}