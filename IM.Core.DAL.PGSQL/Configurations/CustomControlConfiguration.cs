using IM.Core.DAL.Postgres;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations
{
    internal class CustomControlConfiguration : IEntityTypeConfiguration<CustomControl>
    {
        public void Configure(EntityTypeBuilder<CustomControl> builder)
        {
            builder.ToTable("custom_control", Options.Scheme);
            builder.HasKey(x => new {x.UserId, x.ObjectId}).HasName("pk_custom_control");

            builder.Property(e => e.ObjectClass).HasColumnName("object_class_id");
            builder.Property(e => e.ObjectId).HasColumnName("object_id");
            builder.Property(e => e.UserId).HasColumnName("user_id");
        }
    }
}