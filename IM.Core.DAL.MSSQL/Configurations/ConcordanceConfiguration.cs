using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class ConcordanceConfiguration : IEntityTypeConfiguration<Concordance>
    {
        public void Configure(EntityTypeBuilder<Concordance> builder)
        {
            builder.ToTable("IUPConcordance", "dbo");

            builder.HasKey(c => new { c.UrgencyId, c.InfluenceId });

            builder.HasOne(c => c.Influence)
                    .WithMany()
                    .HasForeignKey(c => c.InfluenceId)
                    .HasConstraintName("FK_PriorityMatrixCells_Influence");
            
            builder.HasOne(c => c.Priority)
                    .WithMany()
                    .HasForeignKey(c => c.PriorityID)
                    .HasConstraintName("FK_PriorityMatrixCells_Priority");
        }
    }
}
