using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class MassIncidentNoteConfiguration : NoteConfigurationBase<MassIncident>
    {
        protected override string PrimaryKeyName => "pk_mass_incident_note";

        protected override void ConfigureDatabase(EntityTypeBuilder<Note<MassIncident>> builder)
        {
            builder.ToTable("mass_incident_note", Options.Scheme);

            builder.Property(x => x.NoteText).HasColumnName("text_plain");
            builder.Property(x => x.HTMLNote).HasColumnName("text_formatted");
            builder.Property(x => x.ID).HasColumnName("id");
            builder.Property(x => x.ParentObjectID).HasColumnName("mass_incident_id");
            builder.Property(x => x.Type).HasColumnName("type");
            builder.Property(x => x.UserID).HasColumnName("created_by_user_id");
            builder.Property(x => x.UtcDate).HasColumnName("created_at");
        }
    }
}
