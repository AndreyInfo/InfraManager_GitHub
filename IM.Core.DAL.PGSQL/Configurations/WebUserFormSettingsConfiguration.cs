using IM.Core.DAL.Postgres;
using InfraManager.DAL.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class WebUserFormSettingsConfiguration : IEntityTypeConfiguration<WebUserFormSettings>
    {
        public void Configure(EntityTypeBuilder<WebUserFormSettings> builder)
        {
            builder.ToTable("web_user_form_settings", Options.Scheme);

            builder.HasKey(x => new {x.UserId, x.FormName}).HasName("pk_web_user_form_settings");

            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
                .ValueGeneratedNever();
            builder.Property(x => x.FormName)
                .HasColumnName("form_name")
                .ValueGeneratedNever()
                .HasMaxLength(50);

            builder.Property(x => x.X).HasColumnName("x");
            builder.Property(x => x.Y).HasColumnName("y");
            builder.Property(x => x.Width).HasColumnName("width");
            builder.Property(x => x.Height).HasColumnName("height");
            builder.Property(x => x.Mode).HasColumnName("mode");
        }
    }
}