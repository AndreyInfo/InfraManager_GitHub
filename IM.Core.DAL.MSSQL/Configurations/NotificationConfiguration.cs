using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class NotificationConfiguration : NotificationsConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_Notification";
        protected override string UI_Name => "UI_Name_Notification";

        protected override void ConfigureDatabase(EntityTypeBuilder<Notification.Notification> builder)
        {
            builder.ToTable("Notification", "dbo");

            builder.Property(x => x.ID).HasColumnName("ID");
            builder.Property(x => x.Name).HasColumnName("Name");
            builder.Property(x => x.Note).HasColumnName("Note");
            builder.Property(x => x.Subject).HasColumnType("text").HasColumnName("Subject");
            builder.Property(x => x.Body).HasColumnType("text").HasColumnName("Body");
            builder.Property(x => x.ClassID).HasColumnName("ClassID");
            builder.Property(x => x.AvailableBusinessRole).HasColumnName("AvailableBusinessRole");
            builder.Property(x => x.DefaultBusinessRole).HasColumnName("DefaultBusinessRole");
            builder.Property(x => x.RowVersion).HasColumnName("RowVersion").HasColumnType("timestamp").IsRowVersion();
        }
    }
}
