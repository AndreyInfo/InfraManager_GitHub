using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class NotificationConfiguration : NotificationsConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_notification";
        protected override string UI_Name => "ui_notification_name";

        protected override void ConfigureDatabase(EntityTypeBuilder<Notification.Notification> builder)
        {
            builder.ToTable("notification", Options.Scheme);

            builder.Property(x => x.ID).HasColumnName("id");
            builder.Property(x => x.Name).HasColumnName("name");
            builder.Property(x => x.Note).HasColumnName("note");
            builder.Property(x => x.Subject).HasColumnName("subject");
            builder.Property(x => x.Body).HasColumnName("body");
            builder.Property(x => x.ClassID).HasColumnName("class_id");
            builder.Property(x => x.AvailableBusinessRole).HasColumnName("available_business_role");
            builder.Property(x => x.DefaultBusinessRole).HasColumnName("default_business_role");
            builder.HasXminRowVersion(x => x.RowVersion);
        }
    }
}