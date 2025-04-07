using InfraManager.DAL.ServiceDesk.ChangeRequests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class RFCCategoryConfiguration : IEntityTypeConfiguration<ChangeRequestCategory>
    {
        public void Configure(EntityTypeBuilder<ChangeRequestCategory> builder)
        {
            builder.ToTable("RFCCategory", "dbo");

            builder.Property(x => x.ID)
                .HasColumnName("ID");

            builder.HasKey(x => x.ID);

            builder.Property(x => x.Name)
                .HasColumnName("Name")
                .HasMaxLength(255);

            builder.Property(x => x.Removed)
                .IsRequired()
                .HasColumnName("Removed");
        }
    }
}
