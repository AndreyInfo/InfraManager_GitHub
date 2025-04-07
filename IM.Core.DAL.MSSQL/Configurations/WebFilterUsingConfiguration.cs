using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.Settings;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    public partial class WebFilterUsingConfiguration : IEntityTypeConfiguration<WebFilterUsing>
    {
        public void Configure(EntityTypeBuilder<WebFilterUsing> entity)
        {
            entity.ToTable("WebFilterUsing", "dbo");
            entity.HasKey(e => new { e.FilterId, e.UserId });

            entity.Property(e => e.FilterId).HasColumnName("FilterID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.UtcDateLastUsage)
                .HasColumnName("UtcDateLastUsage")
                .HasColumnType("datetime");

            entity
                .HasOne(x => x.Filter)
                .WithMany()
                .HasForeignKey(x => x.FilterId)
                .HasConstraintName("FK_WebFilterUsing_FilterID")
                .IsRequired();
        }
    }
}
