using IM.Core.DAL.Postgres;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations
{
    internal class RFCCategoryConfiguration : IEntityTypeConfiguration<ChangeRequestCategory>
    {
        public void Configure(EntityTypeBuilder<ChangeRequestCategory> builder)
        {
            builder.ToTable("rfc_category", Options.Scheme);

            builder.Property(x => x.ID)
                .HasColumnName("id");

            builder.HasKey(x => x.ID).HasName("pk_rfc_category");

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(255);

            builder.Property(x => x.Removed)
                .IsRequired()
                .HasColumnName("removed");
        }
    }
}