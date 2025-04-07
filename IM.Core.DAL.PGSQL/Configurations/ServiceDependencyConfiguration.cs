using IM.Core.DAL.Postgres;
using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    public class ServiceDependencyConfiguration : IEntityTypeConfiguration<ServiceDependency>
    {
        public void Configure(EntityTypeBuilder<ServiceDependency> builder)
        {
            builder.ToTable("service_dependency", Options.Scheme);
            builder.HasKey(x => new {x.ParentServiceID, x.ChildServiceID});

            builder.Property(c => c.ParentServiceID)
                .HasColumnName("parent_service_id")
                .IsRequired();

            builder.Property(c => c.ChildServiceID)
                .HasColumnName("child_service_id")
                .IsRequired();


            builder.HasOne(c => c.ParentService)
                .WithMany(c => c.ServiceChildDependency)
                .HasForeignKey(c => c.ParentServiceID);

            builder.HasOne(c => c.ChildService)
                .WithMany(c => c.ServiceParentDependency)
                .HasForeignKey(c => c.ChildServiceID);
        }
    }
}