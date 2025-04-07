using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class ServiceTagConfiguration : IEntityTypeConfiguration<ServiceTag>
    {
        public void Configure(EntityTypeBuilder<ServiceTag> builder)
        {
            builder.ToTable("ServiceTag", "dbo");

            builder.HasKey(c => new { c.ObjectId, c.Tag });

            builder.Property(c => c.ObjectId).HasColumnName("ID")
                                    .ValueGeneratedOnAdd();

            builder.Property(c => c.ClassId).HasColumnName("ClassId");
            builder.Property(c => c.Tag).HasColumnName("Tag");
        }
    }
}
