using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class RFCNoteConfiguration : NoteConfigurationBase<ChangeRequest>
    {
        protected override string PrimaryKeyName => "pk_rfc_note";

        protected override void ConfigureDatabase(EntityTypeBuilder<Note<ChangeRequest>> builder)
        {
            builder.ToTable("rfc_note", Options.Scheme);

            builder.Property(x => x.HTMLNote).HasColumnName("html_note");
            builder.Property(x => x.ID).HasColumnName("id");
            builder.Property(x => x.NoteText).HasColumnName("note");
            builder.Property(x => x.ParentObjectID).HasColumnName("rfc_id");
            builder.Property(x => x.Type).HasColumnName("type");
            builder.Property(x => x.UserID).HasColumnName("user_id");
            builder.Property(x => x.UtcDate).HasColumnName("utc_date");
        }
    }
}