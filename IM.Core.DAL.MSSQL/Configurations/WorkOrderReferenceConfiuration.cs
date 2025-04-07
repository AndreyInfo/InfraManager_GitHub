using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk.WorkOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class WorkOrderReferenceConfiguration : WorkOrderReferenceConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_WorkOrderReference_Auto";

        protected override void ConfigureDatabase(EntityTypeBuilder<WorkOrderReference> builder)
        {
            builder.ToTable("WorkOrderReference", "dbo");

            builder.Property(x => x.ID).HasColumnName("ID");
            builder.Property(x => x.ObjectID).HasColumnName("ObjectID");
            builder.Property(x => x.ObjectClassID).HasColumnName("ClassID");
            builder.Property(x => x.ReferenceName).HasColumnName("ReferenceName");
        }
    }
}
