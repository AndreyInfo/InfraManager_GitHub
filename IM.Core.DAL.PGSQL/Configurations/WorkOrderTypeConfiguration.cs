using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace IM.Core.DAL.Postgres.Configurations
{
    internal class WorkOrderTypeConfiguration : WorkOrderTypeConfigurationBase
    {
        protected override string UI_Name => "ui_work_order_priority_name";
        protected override string PrimaryKeyName => "pk_work_order_type";

        protected override void ConfigureDatabase(EntityTypeBuilder<WorkOrderType> builder)
        {
            builder.ToTable("work_order_type", Options.Scheme);

            builder.Property(e => e.Color).HasColumnName("color");
            builder.Property(e => e.Default).HasColumnName("is_default");
            builder.Property(e => e.FormID).HasColumnName("form_id");
            builder.Property(e => e.IconName).HasColumnName("icon_name");
            builder.Property(e => e.ID).HasColumnName("id");
            builder.Property(e => e.Name).HasColumnName("name");
            builder.Property(e => e.Removed).HasColumnName("removed");
            builder.HasXminRowVersion(e => e.RowVersion);
            builder.Property(e => e.TypeClass).HasColumnName("type_class");
            builder.Property(e => e.WorkflowSchemeIdentifier).HasColumnName("workflow_scheme_identifier");
        }
    }
}