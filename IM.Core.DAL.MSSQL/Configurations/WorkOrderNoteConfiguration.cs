using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class WorkOrderNoteConfiguration : NoteConfigurationBase<WorkOrder>
    {
        protected override string PrimaryKeyName => "PK_WorkOrder";

        protected override void ConfigureDatabase(EntityTypeBuilder<Note<WorkOrder>> builder)
        {
            builder.ToTable("WorkOrderNote", "dbo");

            builder.Property(x => x.HTMLNote).HasColumnName("HTMLNote");
            builder.Property(x => x.ID).HasColumnName("ID");
            builder.Property(x => x.NoteText).HasColumnName("Note");
            builder.Property(x => x.ParentObjectID).HasColumnName("WorkOrderID");
            builder.Property(x => x.Type).HasColumnName("Type");
            builder.Property(x => x.UserID).HasColumnName("UserID");
            builder.Property(x => x.UtcDate).HasColumnName("UtcDate");
        }
    }
}
