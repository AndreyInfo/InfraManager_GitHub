using IM.Core.DAL.Postgres;
using InfraManager.DAL.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations
{
    internal class WebUserColumnSettingsConfiguration : IEntityTypeConfiguration<WebUserColumnSettings>
    {
        public void Configure(EntityTypeBuilder<WebUserColumnSettings> builder)
        {
            builder.ToTable("web_user_column_settings", Options.Scheme);

            builder.HasKey(x => new {x.UserId, x.ListName, x.MemberName}).HasName("pk_web_user_column_settings");

            builder.Property(x => x.UserId).HasColumnName("user_id");
            builder.Property(x => x.ListName).HasMaxLength(50).HasColumnName("list_name");
            builder.Property(x => x.MemberName).HasMaxLength(50).HasColumnName("member_name");
            builder.Property(x => x.Order).HasColumnName("setting_order");
            builder.Property(x => x.CtrlSortAsc).HasColumnName("ctrl_sort_asc");
            builder.Property(x => x.SortAsc).HasColumnName("sort_asc");
            builder.Property(x => x.Visible).HasColumnName("visible");
            builder.Property(x => x.Width).HasColumnName("width");
        }
    }
}