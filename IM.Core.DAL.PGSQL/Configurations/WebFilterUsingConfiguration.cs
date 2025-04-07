using IM.Core.DAL.Postgres;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.Settings;

namespace IM.Core.DAL.PGSQL.Configurations
{
    public partial class WebFilterUsingConfiguration : IEntityTypeConfiguration<WebFilterUsing>
    {
        public void Configure(EntityTypeBuilder<WebFilterUsing> entity)
        {
            entity.ToTable("web_filter_using", Options.Scheme);

            entity.HasKey(e => new {e.FilterId, e.UserId}).HasName("pk_web_filter_using");

            entity.Property(e => e.FilterId).HasColumnName("filter_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.UtcDateLastUsage)
                .HasColumnName("utc_date_last_usage")
                .HasColumnType("datetime");

            entity
                .HasOne(x => x.Filter)
                .WithMany()
                .HasForeignKey(x => x.FilterId)
                .HasConstraintName("fk_web_filter_using_filter_id")
                .IsRequired();
        }
    }
}