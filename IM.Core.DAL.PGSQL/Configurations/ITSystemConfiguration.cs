using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Core.DAL.Postgres;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class ITSystemConfiguration : IEntityTypeConfiguration<ITSystem>
    {
        public void Configure(EntityTypeBuilder<ITSystem> builder)
        {
            builder.ToTable("it_system", Options.Scheme);

            builder.HasKey(c => c.ID);

            builder.Property(c => c.ID).ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWID()")
                .HasColumnName("id");

            builder.Property(c => c.Name).HasMaxLength(250)
                .HasColumnName("name");

            builder.Property(c => c.Note).HasMaxLength(500)
                .HasColumnName("note");

            builder.Property(x => x.RowVersion).HasColumnName("row_version");

            builder.HasXminRowVersion(e => e.RowVersion);

            builder.Property(x => x.DateAnnulated).HasColumnName("date_annulated");
            builder.Property(x => x.DateReceived).HasColumnName("date_received");
            builder.Property(x => x.InfrastructureSegmentID).HasColumnName("infrastructure_segment_id");
            builder.Property(x => x.CriticalityID).HasColumnName("criticality_id");
            builder.Property(x => x.ClientID).HasColumnName("client_id");
            builder.Property(x => x.ClientClassID).HasColumnName("client_class_id");
            builder.Property(x => x.ProductCatalogTypeID).HasColumnName("product_catalog_type_id");

            builder.HasOne(x => x.InfrastructureSegment)
                .WithMany()
                .HasForeignKey(x => x.InfrastructureSegmentID).HasConstraintName("fk_it_system_infrastructure_segment");
        }
    }
}