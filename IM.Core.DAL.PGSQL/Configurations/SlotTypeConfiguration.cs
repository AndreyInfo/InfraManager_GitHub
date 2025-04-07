using IM.Core.DAL.Postgres;
using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class SlotTypeConfiguration : IEntityTypeConfiguration<SlotType>
    {
        public void Configure(EntityTypeBuilder<SlotType> builder)
        {
            builder.ToTable("slot_type", Options.Scheme);

            builder.HasKey(c => c.ID);
            builder.Property(c => c.ID).HasColumnName("identificator");
            builder.Property(c => c.Name).HasColumnName("name");
            builder.Property(c => c.ComplementaryID).HasColumnName("complementary_id");
        }
    }
}