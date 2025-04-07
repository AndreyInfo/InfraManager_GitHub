using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class NotificationsConfigurationBase : IEntityTypeConfiguration<Notification.Notification>
{
    #region Keys

    protected abstract string PrimaryKeyName { get; }
    protected abstract string UI_Name { get; }

    #endregion

    #region configuration

    public void Configure(EntityTypeBuilder<Notification.Notification> builder)
    {
        builder.HasKey(e => e.ID).HasName(PrimaryKeyName);

        builder.Property(x => x.ID).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(250).IsRequired();
        builder.Property(x => x.Note).HasMaxLength(500).IsRequired();
        builder.Property(x => x.Subject).HasMaxLength(500).IsRequired();
        builder.Property(x => x.Subject).IsRequired();
        builder.Property(x => x.Body).IsRequired();
        builder.Property(x => x.ClassID).IsRequired();
        builder.Property(x => x.AvailableBusinessRole).IsRequired();
        builder.Property(x => x.DefaultBusinessRole).IsRequired();

        builder.HasIndex(x => x.Name, UI_Name).IsUnique();

        ConfigureDatabase(builder);
    }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<Notification.Notification> builder);

    #endregion
}