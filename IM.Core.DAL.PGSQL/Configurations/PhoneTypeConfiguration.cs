using IM.Core.DAL.Postgres;
using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class phonetypeConfiguration : IEntityTypeConfiguration<PhoneType>
    {
        public void Configure(EntityTypeBuilder<PhoneType> builder)
        {
            builder.ToTable("telephone_category", Options.Scheme);

            builder.HasKey(c => c.ID);

            builder.Property(c => c.ID).HasColumnName("telephone_category_id");
            builder.Property(c => c.Name).HasColumnName("name");
            builder.Property(c => c.ComplementaryID).HasColumnName("complementary_id");
        }
    }
}