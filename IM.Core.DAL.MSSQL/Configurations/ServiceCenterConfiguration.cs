using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class ServiceCenterConfiguration : IEntityTypeConfiguration<ServiceCenter>
    {
        public void Configure(EntityTypeBuilder<ServiceCenter> builder)
        {
            builder.ToTable("ServiceCenter", "dbo");
            builder.HasKey(x => x.ID);

            builder.Property(c => c.ID).HasColumnName("ServiceCenterID");
            builder.Property(c => c.Name).HasColumnName("Name");
            builder.Property(c => c.Address).HasColumnName("Address");
            builder.Property(c => c.Phone).HasColumnName("Phone");
            builder.Property(c => c.Email).HasColumnName("Email");
            builder.Property(c => c.Manager).HasColumnName("Manager");
            builder.Property(c => c.Notice).HasColumnName("Notice");
            builder.Property(c => c.PeripheralDatabaseID).HasColumnName("PeripheralDatabaseID");
            builder.Property(c => c.ComplementaryID).HasColumnName("ComplementaryID");
            builder.Property(c => c.RowVersion).HasColumnName("RowVersion");
        }
    }
}
