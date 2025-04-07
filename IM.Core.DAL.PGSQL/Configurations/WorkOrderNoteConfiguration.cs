using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class WorkOrderNoteConfiguration : NoteConfigurationBase<WorkOrder>
    {
        protected override string PrimaryKeyName => "pk_work_order_note";

        protected override void ConfigureDatabase(EntityTypeBuilder<Note<WorkOrder>> builder)
        {
            builder.ToTable("work_order_note", Options.Scheme);

            builder.Property(x => x.HTMLNote).HasColumnName("html_note");
            builder.Property(x => x.ID).HasColumnName("id");
            builder.Property(x => x.NoteText).HasColumnName("note");
            builder.Property(x => x.ParentObjectID).HasColumnName("work_order_id");
            builder.Property(x => x.Type).HasColumnName("type");
            builder.Property(x => x.UserID).HasColumnName("user_id");
            builder.Property(x => x.UtcDate).HasColumnName("utc_date");
        }
    }
}