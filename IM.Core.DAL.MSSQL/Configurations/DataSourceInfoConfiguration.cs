using InfraManager.DAL.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class DataSourceInfoConfiguration : IEntityTypeConfiguration<DataSourceInfo>
    {
        public void Configure(EntityTypeBuilder<DataSourceInfo> builder)
        {
            builder.ToTable("DataSourceInfo", "dbo");

            builder.HasKey(x => new { x.OwnerID, x.DataSourceID });

            builder.Property(x => x.OwnerID)
                .HasColumnName("OwnerID");
            builder.Property(x => x.DataSourceID)
                .HasColumnName("DataSourceID");
            builder.Property(x => x.ProcessName)
                .HasColumnName("ProcessName");
            builder.Property(x => x.MachineName)
                .HasColumnName("MachineName");
            builder.Property(x => x.IPAddresses)
                .HasColumnName("IPAddresses");
            builder.Property(x => x.UtcCheckedAt)
                .HasColumnName("UtcCheckedAt");
        }
    }
}
