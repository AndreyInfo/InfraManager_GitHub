using IM.Core.DAL.Postgres;
using InfraManager.DAL.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations
{
    internal class WebUserFilterSettingsConfiguration : IEntityTypeConfiguration<WebUserFilterSettings>
    {
        public void Configure(EntityTypeBuilder<WebUserFilterSettings> builder)
        {
            builder.ToTable("web_user_filter_settings", Options.Scheme);
            //builder.Property(e => e.Filter).HasColumnName("filter_id");

            builder.HasIndex(e => new {e.UserId, e.ViewName, e.Temp},
                "ix_web_user_filter_settings_user_id_view_name_temp");

            builder.HasKey(x => new {x.UserId, x.ViewName});

            builder.Property(x => x.AfterUtcDateModified).HasColumnName("after_utc_date_modified");
            builder.Property(x => x.Temp).HasColumnName("temp");
            builder.Property(x => x.UserId).HasColumnName("user_id");
            builder.Property(x => x.ViewName).HasColumnName("view_name").HasMaxLength(50);
            builder.Property(x => x.WithFinishedWorkflow).HasColumnName("with_finished_workflow");
            builder.HasOne(x => x.Filter).WithMany().HasForeignKey("filter_id");
        }
    }
}