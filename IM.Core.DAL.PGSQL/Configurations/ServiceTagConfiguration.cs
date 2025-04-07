using IM.Core.DAL.Postgres;
using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class ServiceTagConfiguration : IEntityTypeConfiguration<ServiceTag>
    {
        public void Configure(EntityTypeBuilder<ServiceTag> builder)
        {
            builder.ToTable("service_tag", Options.Scheme);

            builder.HasKey(c => new {c.ObjectId, c.Tag});

            builder.Property(c => c.ObjectId).HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(c => c.ClassId).HasColumnName("class_id");
            builder.Property(c => c.Tag).HasColumnName("tag");
        }
    }
}