using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using InfraManager.DAL;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InfraManager.DAL.Notification;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    public class NotificationRecipientConfiguration : IEntityTypeConfiguration<NotificationRecipient>
    {
        public void Configure(EntityTypeBuilder<NotificationRecipient> builder)
        {
            builder.ToTable("NotificationRecipient", "dbo");
            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID).IsRequired();
            builder.Property(x => x.NotificationID).IsRequired();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.Scope).IsRequired();

            builder.HasOne(d => d.Notification)
                 .WithMany(p => p.NotificationRecipients)
                 .HasForeignKey(d => d.NotificationID)
                 .HasConstraintName("FK_NotificationRecipient_Notification")
                 .OnDelete(DeleteBehavior.ClientCascade);

        }
    }
}
