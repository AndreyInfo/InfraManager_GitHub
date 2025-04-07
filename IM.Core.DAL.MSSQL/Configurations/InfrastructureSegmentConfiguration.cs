using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class InfrastructureSegmentConfiguration : IEntityTypeConfiguration<InfrastructureSegment>
    {

        public void Configure(EntityTypeBuilder<InfrastructureSegment> builder)
        {
            builder.ToTable("InfrastructureSegment", "dbo");
            builder.HasKey(c => c.ID);

            builder.Property(c => c.ID).ValueGeneratedOnAdd().HasDefaultValueSql("NEWID()");
            builder.Property(c => c.Name).HasMaxLength(100);
            builder.Property(c => c.RowVersion).IsRowVersion();
        }
    }
}
