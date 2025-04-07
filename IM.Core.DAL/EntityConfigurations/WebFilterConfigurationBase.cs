using InfraManager.DAL.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class WebFilterConfigurationBase : IEntityTypeConfiguration<WebFilter>
    {
        public void Configure(EntityTypeBuilder<WebFilter> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(e => e.Description).HasMaxLength(1000);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(200);
            builder.Property(e => e.ViewName).IsRequired().HasMaxLength(50);

            ConfigureDbProvider(builder);

            builder.Navigation(x => x.Elements).AutoInclude();
        }

        protected abstract void ConfigureDbProvider(EntityTypeBuilder<WebFilter> builder);
    }
}
