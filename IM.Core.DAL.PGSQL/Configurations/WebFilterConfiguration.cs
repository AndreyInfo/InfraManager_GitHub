using IM.Core.DAL.Postgres;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.Settings;
using InfraManager.DAL.EntityConfigurations;

namespace IM.Core.DAL.PGSQL.Configurations
{
    public class WebFilterConfiguration : WebFilterConfigurationBase
    {
        protected override void ConfigureDbProvider(EntityTypeBuilder<WebFilter> entity)
        {
            entity.ToTable("web_filters", Options.Scheme);

            entity.HasKey(x => x.Id).HasName("pk_web_filters");
            entity.HasIndex(e => new {e.UserId, e.ViewName}, "ix_web_filters_user_id_view_name");

            entity.Property(e => e.Others).HasColumnName("others");
            entity.Property(e => e.Standart).HasColumnName("standart");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.ViewName).HasColumnName("view_name");

            entity
                .HasMany(x => x.Elements)
                .WithOne()
                .HasForeignKey("filter_id")
                .HasConstraintName("fk_web_filter_elements_web_filter");
        }
    }
}