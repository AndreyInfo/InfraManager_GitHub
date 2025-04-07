using IM.Core.DAL.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class DataSourceInfoConfiguration : IEntityTypeConfiguration<DataSourceInfo>
    {
        public void Configure(EntityTypeBuilder<DataSourceInfo> builder)
        {
            builder.ToTable("data_source_info", Options.Scheme);
            builder.HasKey(x => new {x.OwnerID, x.DataSourceID});

            builder.Property(x => x.OwnerID)
                .HasColumnName("owner_id");
            builder.Property(x => x.DataSourceID)
                .HasColumnName("data_source_id");
            builder.Property(x => x.ProcessName)
                .HasColumnName("process_name");
            builder.Property(x => x.MachineName)
                .HasColumnName("machine_name");
            builder.Property(x => x.IPAddresses)
                .HasColumnName("ip_addresses");
            builder.Property(x => x.UtcCheckedAt)
                .HasColumnName("utc_checked_at");
        }
    }
}