using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk.WorkOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations
{
    internal class WorkOrderReferenceConfiguration : WorkOrderReferenceConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_work_order_reference_auto";

        protected override void ConfigureDatabase(EntityTypeBuilder<WorkOrderReference> builder)
        {
            builder.ToTable("work_order_reference", Options.Scheme);

            builder.Property(x => x.ID).HasColumnName("id");
            builder.Property(x => x.ObjectID).HasColumnName("object_id");
            builder.Property(x => x.ObjectClassID).HasColumnName("class_id");
            builder.Property(x => x.ReferenceName).HasColumnName("reference_name");
        }
    }
}