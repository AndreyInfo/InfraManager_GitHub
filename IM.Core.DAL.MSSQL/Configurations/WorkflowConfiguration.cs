using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities = InfraManager.DAL.WF;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class WorkflowConfiguration : IEntityTypeConfiguration<Entities.Workflow>
    {
        public void Configure(EntityTypeBuilder<Entities.Workflow> builder)
        {
            builder.ToTable("Workflow", "dbo");
            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID)
                .IsRequired()
                .HasColumnName("ID");
            builder.Property(x => x.WorkflowSchemeID)
                .HasColumnName("WorkflowSchemeID");
            builder.Property(x => x.WorkflowSchemeIdentifier)
                .HasColumnName("WorkflowSchemeIdentifier");
            builder.Property(x => x.WorkflowSchemeVersion)
                .HasColumnName("WorkflowSchemeVersion");
            builder.Property(x => x.EntityClassID)
                .HasColumnName("EntityClassID");
            builder.Property(x => x.EntityID)
                .HasColumnName("EntityID");
            builder.Property(x => x.Status)
                .HasColumnName("Status");
            builder.Property(x => x.CurrentStateID)
                .HasColumnName("CurrentStateID");
            builder.Property(x => x.UtcModifiedAt)
                .HasColumnName("UtcModifiedAt");
            builder.Property(x => x.UtcPlannedAt)
                .HasColumnName("UtcPlannedAt");
            builder.Property(x => x.OwnerID)
                .HasColumnName("OwnerID");
            builder.Property(x => x.UtcOwnedUntil)
                .HasColumnName("UtcOwnedUntil");
            builder.Property(x => x.Binaries)
                .HasColumnName("Binaries");
        }
    }
}
