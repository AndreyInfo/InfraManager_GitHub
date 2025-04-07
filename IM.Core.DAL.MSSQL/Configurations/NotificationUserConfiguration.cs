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
    public class NotificationUserConfiguration : IEntityTypeConfiguration<NotificationUser>
    {
        public void Configure(EntityTypeBuilder<NotificationUser> builder)
        {
            builder.ToTable("NotificationUser", "dbo");
            builder.HasKey(x => new { x.NotificationID, x.UserID });

            builder.Property(x => x.UserID).IsRequired();
            builder.Property(x => x.NotificationID).IsRequired();
            builder.Property(x => x.BusinessRole).IsRequired();

            builder.HasOne(d => d.Notification)
                 .WithMany(p => p.NotificationUsers)
                 .HasForeignKey(d => d.NotificationID)
                 .HasConstraintName("FK_NotificationUser_Notification")
                 .OnDelete(DeleteBehavior.ClientCascade);

        }
    }
}
