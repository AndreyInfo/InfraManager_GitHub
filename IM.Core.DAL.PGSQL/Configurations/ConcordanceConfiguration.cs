using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations
{
    internal class ConcordanceConfiguration : IEntityTypeConfiguration<Concordance>
    {
        public void Configure(EntityTypeBuilder<Concordance> builder)
        {
            builder.ToTable("iup_concordance");

            builder.HasKey(c => new {c.UrgencyId, c.InfluenceId}).HasName("pk_iup_concordance");

            builder.Property(x => x.InfluenceId).HasColumnName("influence_id");
            builder.Property(x => x.PriorityID).HasColumnName("priority_id");
            builder.Property(x => x.UrgencyId).HasColumnName("urgency_id");

            builder.HasOne(c => c.Influence)
                .WithMany()
                .HasForeignKey(x => x.InfluenceId)
                .HasConstraintName("fk_iup_concordance_influence")
                ;

            builder.HasOne(c => c.Priority)
                .WithMany()
                .HasForeignKey(x => x.PriorityID)
                .HasConstraintName("fk_iup_concordance_priority");
        }
    }
}