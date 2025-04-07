using InfraManager.DAL.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class WebUserColumnSettingsConfiguration : IEntityTypeConfiguration<WebUserColumnSettings>
    {
        public void Configure(EntityTypeBuilder<WebUserColumnSettings> builder)
        {
            builder.ToTable("WebUserColumnSettings", "dbo");
            builder.HasKey(x => new { x.UserId, x.ListName, x.MemberName }).HasName("PK_WebUserColumnSettings");

            builder.Property(x => x.UserId).HasColumnName("UserId");
            builder.Property(x => x.ListName).HasMaxLength(50).HasColumnName("ListName");
            builder.Property(x => x.MemberName).HasMaxLength(50).HasColumnName("MemberName");
            builder.Property(x => x.Order).HasColumnName("Order");
            builder.Property(x => x.CtrlSortAsc).HasColumnName("CtrlSortAsc");
            builder.Property(x => x.SortAsc).HasColumnName("SortAsc");
            builder.Property(x => x.Visible).HasColumnName("Visible");
            builder.Property(x => x.Width).HasColumnName("Width");
        }
    }
}
