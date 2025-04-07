using IM.Core.DAL.Postgres;
using InfraManager.DAL.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    public class NotificationRecipientConfiguration : IEntityTypeConfiguration<NotificationRecipient>
    {
        public void Configure(EntityTypeBuilder<NotificationRecipient> builder)
        {
            builder.ToTable("notification_recipient", Options.Scheme);
            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID).HasColumnName("id").IsRequired();
            builder.Property(x => x.NotificationID).HasColumnName("notification_id").IsRequired();
            builder.Property(x => x.Name).HasColumnName("name").IsRequired();
            builder.Property(x => x.Type).HasColumnName("type").IsRequired();
            builder.Property(x => x.Scope).HasColumnName("scope").IsRequired();

            builder.HasOne(d => d.Notification)
                .WithMany(p => p.NotificationRecipients)
                .HasForeignKey(d => d.NotificationID)
                .HasConstraintName("fk_notification_recipient_notification")
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}