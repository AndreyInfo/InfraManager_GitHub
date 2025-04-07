using InfraManager.DAL.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class WebUserFilterSettingsConfiguration : IEntityTypeConfiguration<WebUserFilterSettings>
    {
        public void Configure(EntityTypeBuilder<WebUserFilterSettings> builder)
        {
            builder.ToTable("WebUserFilterSettings", "dbo");
            builder.HasKey(x => new { x.UserId, x.ViewName });

            builder.Property(x => x.AfterUtcDateModified).HasColumnName("AfterUtcDateModified");
            builder.Property(x => x.Temp).HasColumnName("Temp");
            builder.Property(x => x.UserId).HasColumnName("UserId");
            builder.Property(x => x.ViewName).HasColumnName("ViewName").HasMaxLength(50);
            builder.Property(x => x.WithFinishedWorkflow).HasColumnName("WithFinishedWorkflow");
            builder.HasOne(x => x.Filter).WithMany().HasForeignKey("FilterID");
        }
    }
}
