using InfraManager.DAL.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core = InfraManager.DAL.EntityConfigurations;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class EntityEventConfigurationBase : Core.EntityEventConfigurationBase
    {
        protected override void DatabaseConfigure(EntityTypeBuilder<EntityEvent> builder)
        {
            builder.ToTable("EntityEvent", "dbo");

            builder.Property(x => x.Id).HasColumnName("ID");
            builder.Property(x => x.Argument).HasColumnName("Argument").HasColumnType("image");
            builder.Property(x => x.CauserId).HasColumnName("CauserID");
            builder.Property(x => x.EntityClassId).HasColumnName("EntityClassID");
            builder.Property(x => x.EntityId).HasColumnName("EntityID");
            builder.Property(x => x.IsProcessed).HasColumnName("IsProcessed");
            builder.Property(x => x.Order)
                .HasColumnName("Order")
                .HasDefaultValueSql("NEXT VALUE FOR [dbo].[ExternalEventNumber]");
            builder.Property(x => x.OwnerId).HasColumnName("OwnerID");
            builder.Property(x => x.Source).HasColumnName("Source");
            builder.Property(x => x.Type).HasColumnName("Type");
            builder.Property(x => x.UtcOwnedUntil).HasColumnName("UtcOwnedUntil").HasColumnType("datetime");
            builder.Property(x => x.UtcRegisteredAt).HasColumnName("UtcRegisteredAt").HasColumnType("datetime");
        }
    }
}
