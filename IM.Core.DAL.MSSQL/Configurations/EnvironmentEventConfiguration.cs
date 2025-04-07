using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class EnvironmentEventConfiguration : IEntityTypeConfiguration<EnvironmentEvent>
    {
        public void Configure(EntityTypeBuilder<EnvironmentEvent> builder)
        {
            builder.ToTable("EnvironmentEvent", "dbo");
            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID)
                .HasColumnName("ID");
            builder.Property(x => x.UtcRegisteredAt)
                .HasColumnName("UtcRegisteredAt");
            builder.Property(x => x.Order)
                .HasColumnName("Order");
            builder.Property(x => x.Source)
                .HasColumnName("Type");
            builder.Property(x => x.Type)
                .HasColumnName("type");
            builder.Property(x => x.OwnerID)
                .HasColumnName("OwnerID");
            builder.Property(x => x.UtcOwnedUntil)
                .HasColumnName("UtcOwnedUntil");
            builder.Property(x => x.CauserID)
                .HasColumnName("CauserID");
            builder.Property(x => x.IsProcessed)
                .HasColumnName("IsProcessed");
            builder.Property(x => x.WorkflowSchemeID)
                .HasColumnName("WorkflowSchemeID");
        }
    }
}
