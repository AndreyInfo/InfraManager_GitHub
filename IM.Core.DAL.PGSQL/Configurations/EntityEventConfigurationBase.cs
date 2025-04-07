using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations
{
    internal class EntityEventConfiguration : EntityEventConfigurationBase
    {
        protected override void DatabaseConfigure(EntityTypeBuilder<EntityEvent> builder)
        {
            builder.ToTable("entity_event", Options.Scheme);
            builder.Property(e => e.Argument).HasColumnName("argument").HasColumnType("BYTEA");
            builder.Property(e => e.CauserId).HasColumnName("causer_id").HasColumnType("UUID");
            builder.Property(e => e.EntityClassId).HasColumnName("entity_class_id").HasColumnType("INT");
            builder.Property(e => e.EntityId).HasColumnName("entity_id").HasColumnType("UUID");
            builder.Property(e => e.Id).HasColumnName("id").HasColumnType("UUID");
            builder.Property(e => e.IsProcessed).HasColumnName("is_processed").HasColumnType("BOOLEAN");
            builder.Property(e => e.Order).HasColumnName("event_order").HasColumnType("BIGINT")
                .HasDefaultValueSql("nextval('external_event_number]')");
            builder.Property(e => e.OwnerId).HasColumnName("owner_id").HasColumnType("UUID");
            builder.Property(e => e.Source).HasColumnName("source").HasColumnType("INT");
            builder.Property(e => e.TargetStateId).HasColumnName("target_state_id").HasColumnType("VARCHAR(100)");
            builder.Property(e => e.Type).HasColumnName("type").HasColumnType("INT");
            builder.Property(e => e.UtcOwnedUntil).HasColumnName("utc_owned_until").HasColumnType("TIMESTAMP(3)");
            builder.Property(e => e.UtcRegisteredAt).HasColumnName("utc_registered_at").HasColumnType("TIMESTAMP(3)");
            // EntityEvent => entity_event
            builder.HasKey(e => e.Id).HasName("pk_entity_event");
        }
    }
}