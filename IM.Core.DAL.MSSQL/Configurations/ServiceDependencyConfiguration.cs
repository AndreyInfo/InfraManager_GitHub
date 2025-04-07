using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    public class ServiceDependencyConfiguration : IEntityTypeConfiguration<ServiceDependency>
    {
        public void Configure(EntityTypeBuilder<ServiceDependency> builder)
        {
            builder.ToTable("ServiceDependency", "dbo");
            builder.HasKey(x => new { x.ParentServiceID, x.ChildServiceID });

            builder.HasOne(c => c.ParentService)
                .WithMany(c => c.ServiceChildDependency)
                .HasForeignKey(c => c.ParentServiceID);

            builder.HasOne(c => c.ChildService)
                .WithMany(c => c.ServiceParentDependency)
                .HasForeignKey(c => c.ChildServiceID);
        }
    }
}
