using IM.Core.DAL.Postgres;
using InfraManager.DAL.Postgres;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class WorkFlowSchemeConfigure : IEntityTypeConfiguration<WorkFlowScheme>
    {
        public void Configure(EntityTypeBuilder<WorkFlowScheme> builder)
        {
            builder.ToTable("workflow_scheme", Options.Scheme);
            builder.HasKey(x => x.Id).HasName("pk_workflow_scheme");
            ConfigureProperty(builder);
            ConfigureForeignEntities(builder);
        }

        public void ConfigureProperty(EntityTypeBuilder<WorkFlowScheme> builder)
        {
            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Identifier).HasColumnName("identifier");
            builder.Property(e => e.LogicalScheme).HasColumnName("logical_scheme");
            builder.Property(e => e.MajorVersion).HasColumnName("major_version");
            builder.Property(e => e.MinorVersion).HasColumnName("minor_version");
            builder.Property(e => e.ModifierID).HasColumnName("modifier_id");
            builder.Property(e => e.Name).HasColumnName("name");
            builder.Property(e => e.Note).HasColumnName("note");
            builder.Property(e => e.ObjectClassID).HasColumnName("object_class_id");
            builder.Property(e => e.PublisherID).HasColumnName("publisher_id");
            builder.HasXminRowVersion(e => e.RowVersion);
            builder.Property(e => e.Status).HasColumnName("status");
            builder.Property(e => e.TraceIsEnabled).HasColumnName("trace_is_enabled");
            builder.Property(e => e.UtcDateModified).HasColumnName("utc_date_modified");
            builder.Property(e => e.UtcDatePublished).HasColumnName("utc_date_published");
            builder.Property(e => e.VisualScheme).HasColumnName("visual_scheme");
            builder.Property(e => e.WorkflowSchemeFolderID).HasColumnName("workflow_scheme_folder_id");

            builder.Property(x => x.Identifier).HasMaxLength(200);
        }

        public void ConfigureForeignEntities(EntityTypeBuilder<WorkFlowScheme> builder)
        {
            builder.HasMany(c => c.ProblemTypes)
                .WithOne(c => c.WorkflowScheme)
                .HasForeignKey(c => c.WorkflowSchemeIdentifier)
                .HasPrincipalKey(c => c.Identifier)
                .HasConstraintName("fk_workflow_scheme_workflow_scheme_folder");
        }
    }
}