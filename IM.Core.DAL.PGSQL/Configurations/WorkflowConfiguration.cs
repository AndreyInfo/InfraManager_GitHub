using IM.Core.DAL.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities = InfraManager.DAL.WF;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class WorkflowConfiguration : IEntityTypeConfiguration<Entities.Workflow>
    {
        public void Configure(EntityTypeBuilder<Entities.Workflow> builder)
        {
            builder.ToTable("workflow", Options.Scheme);
            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID)
                .IsRequired()
                .HasColumnName("id");
            builder.Property(x => x.WorkflowSchemeID)
                .HasColumnName("workflow_scheme_id");
            builder.Property(x => x.WorkflowSchemeIdentifier)
                .HasColumnName("workflow_scheme_identifier");
            builder.Property(x => x.WorkflowSchemeVersion)
                .HasColumnName("workflow_scheme_version");
            builder.Property(x => x.EntityClassID)
                .HasColumnName("entity_class_id");
            builder.Property(x => x.EntityID)
                .HasColumnName("entity_id");
            builder.Property(x => x.Status)
                .HasColumnName("status");
            builder.Property(x => x.CurrentStateID)
                .HasColumnName("current_state_id");
            builder.Property(x => x.UtcModifiedAt)
                .HasColumnName("utc_modified_at");
            builder.Property(x => x.UtcPlannedAt)
                .HasColumnName("utc_planned_at");
            builder.Property(x => x.OwnerID)
                .HasColumnName("owner_id");
            builder.Property(x => x.UtcOwnedUntil)
                .HasColumnName("utc_owned_until");
            builder.Property(x => x.Binaries)
                .HasColumnName("binaries");
        }
    }
}