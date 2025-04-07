using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    public class CallTypeConfiguration : CallTypeBaseConfigurationBase
    {
        protected override void ConfigureDatabase(EntityTypeBuilder<CallType> builder)
        {
            builder.ToTable("CallType", "dbo");

            builder.Property(x => x.ID).HasColumnName("ID");
            builder.Property(x => x.Name).HasColumnName("Name");
            builder.Property(x => x.VisibleInWeb).HasColumnName("VisibleInWeb");
            builder.Property(x => x.EventHandlerCallTypeID).HasColumnName("EventHandlerCallTypeID");
            builder.Property(x => x.EventHandlerName).HasColumnName("EventHandlerName");
            builder.Property(x => x.Icon).HasColumnName("Icon").HasColumnType("image");
            builder.Property(x => x.IconName).HasColumnName("IconName");
            builder.Property(x => x.ParentCallTypeID).HasColumnName("ParentCallTypeID");
            builder.Property(x => x.RowVersion).IsRowVersion().HasColumnName("RowVersion").HasColumnType("timestamp");
            builder.Property(x => x.WorkflowSchemeIdentifier).HasColumnName("WorkflowSchemeIdentifier");
            builder.Property(x => x.UseWorkflowSchemeFromAttendance).HasColumnName("UseWorkflowSchemeFromAttendance");
            builder.Property(x => x.Removed).HasColumnName("Removed");
            builder.Property(x => x.IsFixed).HasColumnName("IsFixed");
        }

        protected override string PrimaryKeyName => "PK_CallType";
        protected override string UI_Name_ParentCallTypeID => "UI_CallType_Name_ParentCallTypeID";
    }
}
