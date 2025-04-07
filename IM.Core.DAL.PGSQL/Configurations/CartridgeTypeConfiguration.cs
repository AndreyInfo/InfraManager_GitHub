using IM.Core.DAL.Postgres;
using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class CartridgeTypeConfiguration : IEntityTypeConfiguration<CartridgeType>
    {
        public void Configure(EntityTypeBuilder<CartridgeType> builder)
        {
            builder.ToTable("cartridge_types", Options.Scheme);
            builder.HasKey(c => c.ID);
            builder.Property(c => c.ID).HasColumnName("cartridge_type_id");
            builder.Property(c => c.Name).HasColumnName("name");
            builder.Property(c => c.ComplementaryID).HasColumnName("complementary_id");
        }
    }
}