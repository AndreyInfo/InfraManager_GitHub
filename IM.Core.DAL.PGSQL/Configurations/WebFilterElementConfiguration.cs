using IM.Core.DAL.Postgres;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.Settings;

namespace IM.Core.DAL.PGSQL.Configurations
{
    public partial class WebFilterElementConfiguration : IEntityTypeConfiguration<WebFilterElement>
    {
        public void Configure(EntityTypeBuilder<WebFilterElement> entity)
        {
            entity.ToTable("web_filter_elements", Options.Scheme);

            entity.HasKey(x => x.Id).HasName("pk_web_filter_elements");
            entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedNever();
            entity.Property(e => e.Data).HasColumnName("data").IsRequired();
        }
    }
}