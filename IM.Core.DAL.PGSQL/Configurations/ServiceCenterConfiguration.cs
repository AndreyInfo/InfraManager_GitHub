using InfraManager.DAL.Postgres;
using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations
{
    internal class ServiceCenterConfiguration : IEntityTypeConfiguration<ServiceCenter>
    {
        public void Configure(EntityTypeBuilder<ServiceCenter> builder)
        {
            builder.ToTable("service_center", Options.Scheme);
            builder.HasKey(x => x.ID);

            builder.Property(c => c.ID).HasColumnName("service_center_id");
            builder.Property(c => c.Name).HasColumnName("name");
            builder.Property(c => c.Address).HasColumnName("address");
            builder.Property(c => c.Phone).HasColumnName("phone");
            builder.Property(c => c.Email).HasColumnName("email");
            builder.Property(c => c.Manager).HasColumnName("manager");
            builder.Property(c => c.Notice).HasColumnName("notice");
            builder.Property(c => c.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
            builder.Property(c => c.ComplementaryID).HasColumnName("complementary_id");
            builder.HasXminRowVersion(c => c.RowVersion);
        }
    }
}