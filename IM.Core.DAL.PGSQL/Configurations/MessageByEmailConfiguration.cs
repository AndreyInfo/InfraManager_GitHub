using IM.Core.DAL.Postgres;
using InfraManager.DAL.Message;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations
{
    internal class MessageByEmailConfiguration : IEntityTypeConfiguration<MessageByEmail>
    {
        public void Configure(EntityTypeBuilder<MessageByEmail> builder)
        {
            builder.ToTable("message_by_email", Options.Scheme);
            builder.HasKey(x => x.IMObjID);

            ConfigureColumns(builder);
            ConfigureNavigationProperties(builder);
        }


        private void ConfigureColumns(EntityTypeBuilder<MessageByEmail> builder)
        {
            builder.Property(x => x.IMObjID).HasColumnName("message_id").IsRequired();
            builder.Property(x => x.From).HasColumnName("from_").IsRequired().HasMaxLength(500);
            builder.Property(x => x.To).HasColumnName("to_").IsRequired().HasMaxLength(500);
            builder.Property(x => x.Title).HasColumnName("title").HasMaxLength(500);
            builder.Property(x => x.Content).HasColumnName("content").IsRequired().HasColumnType("text");
            builder.Property(x => x.HtmlContent).HasColumnName("html_content").IsRequired().HasColumnType("text");
            builder.Property(x => x.MessageMimeId).HasColumnName("message_mime_id").HasMaxLength(500);
            builder.Property(x => x.UtcDateReceived).HasColumnName("utc_date_received");

            builder.Ignore(x => x.UtcDateModified);
            builder.Ignore(x => x.EntityStateID);
            builder.Ignore(x => x.EntityStateName);
            builder.Ignore(x => x.WorkflowSchemeID);
            builder.Ignore(x => x.WorkflowSchemeVersion);
            builder.Ignore(x => x.WorkflowSchemeIdentifier);
            builder.Ignore(x => x.TargetEntityStateID);

        }

        private void ConfigureNavigationProperties(EntityTypeBuilder<MessageByEmail> builder)
        {
            builder.HasOne(x => x.Message)
                .WithMany()
                .HasForeignKey(x => x.IMObjID)
                .HasConstraintName("fk_message_by_email_message");
        }
    }
}
