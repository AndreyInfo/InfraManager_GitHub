using InfraManager.DAL.Message;
using InfraManager.DAL.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations
{
    internal class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("message", Options.Scheme);
            builder.HasKey(x => x.IMObjID);

            ConfigureColumns(builder);
            ConfigureNavigationProperties(builder);
        }

        private void ConfigureColumns(EntityTypeBuilder<Message> builder)
        {
            builder.Property(x => x.IMObjID).HasColumnName("id");
            builder.Property(x => x.UtcDateRegistered).HasColumnName("utc_date_registered").IsRequired();
            builder.Property(x => x.UtcDateClosed).HasColumnName("utc_date_closed");
            builder.Ignore(x => x.UtcDateModified);
            builder.Property(x => x.Count).HasColumnName("count").IsRequired();
            builder.Property(x => x.Type).HasColumnName("type").IsRequired();
            builder.Property(x => x.SeverityType).HasColumnName("severity_type").IsRequired();
            builder.HasXminRowVersion(e => e.RowVersion);

            builder.Property(x => x.EntityStateID).HasColumnName("entity_state_id").HasMaxLength(50);
            builder.Property(x => x.EntityStateName).HasColumnName("entity_state_name").HasMaxLength(250);
            builder.Property(x => x.WorkflowSchemeID).HasColumnName("workflow_scheme_id");
            builder.Property(x => x.WorkflowSchemeIdentifier).HasColumnName("workflow_scheme_identifier")
                .HasMaxLength(50);
            builder.Property(x => x.WorkflowSchemeVersion).HasColumnName("workflow_scheme_version").HasMaxLength(50);
            builder.Ignore(x => x.TargetEntityStateID);
        }

        private void ConfigureNavigationProperties(EntityTypeBuilder<Message> builder)
        {
        }
    }
}