using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class CriticalityConfiguration : IEntityTypeConfiguration<Criticality>
    {
        public void Configure(EntityTypeBuilder<Criticality> builder)
        {
            builder.ToTable("Criticality", "dbo");

            builder.HasKey(c => c.ID);

            builder.Property(c => c.ID).HasColumnName("ID");
            builder.Property(c => c.Name).HasColumnName("Name");
            builder.Property(c => c.RowVersion)
                .HasColumnName("RowVersion")
                .IsRowVersion();
        }
    }
}
