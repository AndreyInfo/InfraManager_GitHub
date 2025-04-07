using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class ServiceCategoryConfiguration : LookupConfiguration<ServiceCategory>
    {
        protected override string TableName => "ServiceCategory";

        protected override void ConfigureAdditionalMembers(EntityTypeBuilder<ServiceCategory> builder)
        {
            builder.HasIndex(c => c.Name, "UI_ServiceCategory_Name").IsUnique();
            builder.Property(x => x.Note).HasColumnName("Note").HasColumnType("text").IsRequired();
            builder.Property(x => x.Icon).HasColumnName("Icon").HasColumnType("image");
            builder.Property(x => x.ExternalId).HasColumnName("ExternalID").IsRequired();

            builder.HasMany(c => c.Services)
                .WithOne(c => c.Category)
                .HasForeignKey(c => c.CategoryID);
        }
    }
}
