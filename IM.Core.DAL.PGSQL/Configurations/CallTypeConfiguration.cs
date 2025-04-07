using IM.Core.DAL.Postgres;
using InfraManager.DAL.Postgres;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InfraManager.DAL.EntityConfigurations;

namespace IM.Core.DAL.PGSQL.Configurations
{
    internal class CallTypeConfiguration : CallTypeBaseConfigurationBase
    {
        protected override void ConfigureDatabase(EntityTypeBuilder<CallType> builder)
        {
            builder.ToTable("call_type", Options.Scheme);

            builder.Property(x => x.ID).HasColumnName("id");
            builder.Property(x => x.Name).HasColumnName("name");
            builder.Property(x => x.VisibleInWeb).HasColumnName("visible_in_web");
            builder.Property(x => x.EventHandlerCallTypeID).HasColumnName("event_handler_call_type_id");
            builder.Property(x => x.EventHandlerName).HasColumnName("event_handler_name");
            builder.Property(x => x.Icon).HasColumnName("icon").HasColumnType("image");
            builder.Property(x => x.IsFixed).HasColumnName("is_fixed");
            builder.HasXminRowVersion(e => e.RowVersion);
            builder.Property(x => x.WorkflowSchemeIdentifier).HasColumnName("workflow_scheme_identifier")
                ;
            builder.Property(x => x.UseWorkflowSchemeFromAttendance)
                .HasColumnName("use_workflow_scheme_from_attendance");
            builder.Property(x => x.ParentCallTypeID).HasColumnName("parent_call_type_id");
            builder.Property(x => x.Removed).HasColumnName("removed");
            builder.Property(x => x.IconName).HasColumnName("icon_name");
        }

        protected override string PrimaryKeyName => "pk_call_type";
        protected override string UI_Name_ParentCallTypeID => "ui_name_parent_call_type_id";
    }
}