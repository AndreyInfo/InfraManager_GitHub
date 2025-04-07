using IM.Core.DAL.Postgres;
using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class InfrastructureSegmentConfiguration : IEntityTypeConfiguration<InfrastructureSegment>
    {
        public void Configure(EntityTypeBuilder<InfrastructureSegment> builder)
        {
            builder.ToTable("infrastructure_segment", Options.Scheme);
            builder.HasKey(c => c.ID);

            builder.Property(c => c.ID).ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWID()")
                .HasColumnName("id");

            builder.Property(c => c.Name).HasMaxLength(100).HasColumnName("name");

            builder.HasXminRowVersion(x => x.RowVersion);
        }
    }
}