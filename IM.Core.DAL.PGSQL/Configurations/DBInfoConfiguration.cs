using IM.Core.DAL.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class DBInfoConfiguration : IEntityTypeConfiguration<DBInfo>
    {
        public void Configure(EntityTypeBuilder<DBInfo> builder)
        {
            builder.ToTable("db_info", Options.Scheme);
            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID)
                .HasColumnName("id");
            builder.Property(x => x.Version)
                .HasColumnName("version");
            builder.Property(x => x.IsPeripheral)
                .HasColumnName("is_peripheral");
            builder.Property(x => x.IsCentral)
                .HasColumnName("is_central");
            builder.Property(x => x.SIDControl)
                .HasColumnName("s_id_control");
        }
    }
}