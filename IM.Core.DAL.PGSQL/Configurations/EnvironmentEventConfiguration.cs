using IM.Core.DAL.Postgres;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class EnvironmentEventConfiguration : IEntityTypeConfiguration<EnvironmentEvent>
    {
        public void Configure(EntityTypeBuilder<EnvironmentEvent> builder)
        {
            builder.ToTable("environment_event", Options.Scheme);
            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID)
                .HasColumnName("id");
            builder.Property(x => x.UtcRegisteredAt)
                .HasColumnName("utc_registered_at");
            builder.Property(x => x.Order)
                .HasColumnName("event_order");
            builder.Property(x => x.Source)
                .HasColumnName("source");
            builder.Property(x => x.Type)
                .HasColumnName("type");
            builder.Property(x => x.OwnerID)
                .HasColumnName("owner_id");
            builder.Property(x => x.UtcOwnedUntil)
                .HasColumnName("utc_owned_until");
            builder.Property(x => x.CauserID)
                .HasColumnName("causer_id");
            builder.Property(x => x.IsProcessed)
                .HasColumnName("is_processed");
            builder.Property(x => x.WorkflowSchemeID)
                .HasColumnName("workflow_scheme_id");
        }
    }
}