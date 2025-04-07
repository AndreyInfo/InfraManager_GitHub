using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class RFCNoteConfiguration : NoteConfigurationBase<ChangeRequest>
    {
        protected override string PrimaryKeyName => "PK_RFCNote";

        protected override void ConfigureDatabase(EntityTypeBuilder<Note<ChangeRequest>> builder)
        {
            builder.ToTable("RFCNote", "dbo");

            builder.Property(x => x.ID).HasColumnName("Id");
            builder.Property(x => x.NoteText).HasColumnName("Note");
            builder.Property(x => x.HTMLNote).HasColumnName("HTMLNote");
            builder.Property(x => x.ParentObjectID).HasColumnName("RFCID");
            builder.Property(x => x.Type).HasColumnName("Type");
            builder.Property(x => x.UserID).HasColumnName("UserID");
            builder.Property(x => x.UtcDate).HasColumnName("UtcDate").HasColumnType("datetime");
        }
    }
}
