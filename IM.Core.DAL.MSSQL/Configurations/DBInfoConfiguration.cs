using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class DBInfoConfiguration : IEntityTypeConfiguration<DBInfo>
    {
        public void Configure(EntityTypeBuilder<DBInfo> builder)
        {
            builder.ToTable("DBInfo", "dbo");
            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID)
                   .HasColumnName("ID");
            builder.Property(x => x.Version)
                   .HasColumnName("Version");
            builder.Property(x => x.IsPeripheral)
                   .HasColumnName("IsPeripheral");
            builder.Property(x => x.IsCentral)
                   .HasColumnName("IsCentral");
            builder.Property(x => x.SIDControl)
                   .HasColumnName("SIDControl");
        }
    }
}
