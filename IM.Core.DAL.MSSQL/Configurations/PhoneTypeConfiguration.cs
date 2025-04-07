using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class PhoneTypeConfiguration : IEntityTypeConfiguration<PhoneType>
    {
        public void Configure(EntityTypeBuilder<PhoneType> builder)
        {
            builder.ToTable("TelephoneCategory", "dbo");

            builder.HasKey(c => c.ID);

            builder.Property(c => c.ID).HasColumnName("TelephoneCategoryID");
        }
    }
}
