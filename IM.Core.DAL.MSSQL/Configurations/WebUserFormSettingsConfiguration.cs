using InfraManager.DAL.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class WebUserFormSettingsConfiguration : IEntityTypeConfiguration<WebUserFormSettings>
    {
        public void Configure(EntityTypeBuilder<WebUserFormSettings> builder)
        {
            builder.ToTable("WebUserFormSettings", "dbo");
            builder.HasKey(x => new { x.UserId, x.FormName });

            builder.Property(x => x.UserId)
                .HasColumnName("UserID")
                .ValueGeneratedNever();
            builder.Property(x => x.FormName)
                .HasColumnName("FormName")
                .ValueGeneratedNever()
                .HasMaxLength(50);

            builder.Property(x => x.X).HasColumnName("X");
            builder.Property(x => x.Y).HasColumnName("Y");
            builder.Property(x => x.Width).HasColumnName("Width");
            builder.Property(x => x.Height).HasColumnName("Height");
            builder.Property(x => x.Mode).HasColumnName("Mode");
        }
    }
}
