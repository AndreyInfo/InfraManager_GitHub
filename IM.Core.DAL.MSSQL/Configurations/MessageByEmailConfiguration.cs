using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class MessageByEmailConfiguration : IEntityTypeConfiguration<Message.MessageByEmail>
    {
        public void Configure(EntityTypeBuilder<Message.MessageByEmail> builder)
        {
            builder.ToTable("MessageByEmail", "dbo");
            builder.HasKey(x => x.IMObjID);

            ConfigureColumns(builder);
            ConfigureNavigationProperties(builder);
        }

        private void ConfigureColumns(EntityTypeBuilder<Message.MessageByEmail> builder)
        {
            builder.Property(x => x.IMObjID).HasColumnName("MessageID").IsRequired();
            builder.Property(x => x.From).HasColumnName("From").IsRequired().HasMaxLength(500);
            builder.Property(x => x.To).HasColumnName("To").IsRequired().HasMaxLength(500);
            builder.Property(x => x.Title).HasColumnName("Title").HasMaxLength(500);
            builder.Property(x => x.Content).HasColumnName("Content").IsRequired().HasColumnType("text");
            builder.Property(x => x.HtmlContent).HasColumnName("HtmlContent").IsRequired().HasColumnType("nvarchar(max)");
            builder.Property(x => x.MessageMimeId).HasColumnName("MessageMimeId").HasMaxLength(500);
            builder.Property(x => x.UtcDateReceived).HasColumnName("UtcDateReceived");

            builder.Ignore(x => x.UtcDateModified);
            builder.Ignore(x => x.EntityStateID);
            builder.Ignore(x => x.EntityStateName);
            builder.Ignore(x => x.WorkflowSchemeID);
            builder.Ignore(x => x.WorkflowSchemeVersion);
            builder.Ignore(x => x.WorkflowSchemeIdentifier);
            builder.Ignore(x => x.TargetEntityStateID);

        }

        private void ConfigureNavigationProperties(EntityTypeBuilder<Message.MessageByEmail> builder)
        {
            builder.HasOne(x => x.Message)
                .WithMany()
                .HasForeignKey(x=>x.IMObjID)
                .HasConstraintName("FK_MessageByEmail_Message");
        }
    }
}
