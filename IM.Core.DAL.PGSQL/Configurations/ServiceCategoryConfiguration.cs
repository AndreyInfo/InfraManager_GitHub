using InfraManager.DAL.Postgres;
using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class ServiceCategoryConfiguration : LookupConfiguration<ServiceCategory>
    {
        protected override string TableName => "service_category";

        protected override void ConfigureAdditionalMembers(EntityTypeBuilder<ServiceCategory> builder)
        {
            builder.HasIndex(c => c.Name, "ui_service_category_name").IsUnique();
            builder.Property(x => x.Note).HasColumnName("note").HasColumnType("text").IsRequired();
            builder.Property(x => x.Icon).HasColumnName("icon").HasColumnType("bytea");
            builder.Property(x => x.IconName).HasColumnName("icon_name");
            builder.Property(x => x.ExternalId).HasColumnName("external_id").IsRequired();

            builder.HasMany(x => x.Services)
                .WithOne(x => x.Category)
                .HasForeignKey(x => x.CategoryID);
        }
    }
}