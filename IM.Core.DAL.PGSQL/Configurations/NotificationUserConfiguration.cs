using IM.Core.DAL.Postgres;
using InfraManager.DAL.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    public class NotificationUserConfiguration : IEntityTypeConfiguration<NotificationUser>
    {
        public void Configure(EntityTypeBuilder<NotificationUser> builder)
        {
            builder.ToTable("notification_user", Options.Scheme);
            builder.HasKey(x => new {x.NotificationID, x.UserID});

            builder.Property(x => x.UserID).HasColumnName("user_id").IsRequired();
            builder.Property(x => x.NotificationID).HasColumnName("notification_id").IsRequired();
            builder.Property(x => x.BusinessRole).HasColumnName("business_role").IsRequired();

            builder.HasOne(d => d.Notification)
                .WithMany(p => p.NotificationUsers)
                .HasForeignKey(d => d.NotificationID)
                .HasConstraintName("fk_notification_user_notification")
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}