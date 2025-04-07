using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class SlotTypeConfiguration : IEntityTypeConfiguration<SlotType>
    {
        public void Configure(EntityTypeBuilder<SlotType> builder)
        {
            builder.ToTable("Тип слота", "dbo");

            builder.HasKey(c => c.ID);
            builder.Property(c => c.ID).HasColumnName("Идентификатор");
            builder.Property(c => c.Name).HasColumnName("Название");
        }
    }
}
