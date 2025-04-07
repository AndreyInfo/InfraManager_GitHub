using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.Settings;
using InfraManager.DAL.EntityConfigurations;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    public class WebFilterConfiguration : WebFilterConfigurationBase
    {
        protected override void ConfigureDbProvider(EntityTypeBuilder<WebFilter> entity)
        {
            entity.ToTable("WebFilters", "dbo");
            entity.HasIndex(e => new { e.UserId, e.ViewName }, "IX_WebFilters_UserID_ViewName");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description).HasColumnName("Description");
            entity.Property(e => e.Name).HasColumnName("Name");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.ViewName).HasColumnName("ViewName");

            entity
                .HasMany(x => x.Elements)
                .WithOne()
                .HasForeignKey("FilterID")
                .HasConstraintName("FK_WebFilterElements_WebFilter");
        }
    }
}
