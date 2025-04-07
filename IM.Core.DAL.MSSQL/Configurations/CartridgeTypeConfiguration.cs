using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class CartridgeTypeConfiguration : IEntityTypeConfiguration<CartridgeType>
    {
        public void Configure(EntityTypeBuilder<CartridgeType> builder)
        {
            builder.ToTable("CartridgeTypes", "dbo");
            builder.HasKey(c => c.ID);
            builder.Property(c => c.ID).HasColumnName("CartridgeTypeID");
        }
    }
}
