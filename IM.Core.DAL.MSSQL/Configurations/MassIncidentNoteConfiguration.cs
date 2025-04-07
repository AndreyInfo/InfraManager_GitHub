using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class MassIncidentNoteConfiguration : NoteConfigurationBase<MassIncident>
    {
        protected override string PrimaryKeyName => "PK_MassIncidentNote";

        protected override void ConfigureDatabase(EntityTypeBuilder<Note<MassIncident>> builder)
        {
            builder.ToTable("MassIncidentNote", "dbo");

            builder.Property(x => x.NoteText).HasColumnName("TextPlain");
            builder.Property(x => x.HTMLNote).HasColumnName("TextFormatted");
            builder.Property(x => x.ID).HasColumnName("ID");
            builder.Property(x => x.ParentObjectID).HasColumnName("MassIncidentID");
            builder.Property(x => x.Type).HasColumnName("Type");
            builder.Property(x => x.UserID).HasColumnName("CreatedByUserID");
            builder.Property(x => x.UtcDate).HasColumnName("CreatedAt");
        }
    }
}
