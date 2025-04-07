using IM.Core.DAL.Postgres;
using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class CriticalityConfiguration : IEntityTypeConfiguration<Criticality>
    {
        public void Configure(EntityTypeBuilder<Criticality> builder)
        {
            builder.ToTable("criticality", Options.Scheme);

            builder.HasKey(c => c.ID);

            builder.Property(c => c.ID).HasColumnName("id");
            builder.Property(c => c.Name).HasColumnName("name");
            builder.Property(c => c.RowVersion).HasColumnName("row_version");


            builder.HasXminRowVersion(x => x.RowVersion);
        }
    }
}