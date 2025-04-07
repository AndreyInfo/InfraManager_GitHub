using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class CustomControlConfiguration : IEntityTypeConfiguration<CustomControl>
    {
        public void Configure(EntityTypeBuilder<CustomControl> builder)
        {
            builder.ToTable("CustomControl", "dbo");
            builder.HasKey(x => new { x.UserId, x.ObjectId });

            builder.Property(x => x.UserId).HasColumnName("UserId");
            builder.Property(x => x.ObjectId).HasColumnName("ObjectId");
            builder.Property(x => x.ObjectClass).HasColumnName("ObjectClassId");
        }
    }
}
