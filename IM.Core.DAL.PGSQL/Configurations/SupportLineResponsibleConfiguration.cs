using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;

internal sealed class SupportLineResponsibleConfiguration : SupportLineResponsibleConfigurationBase
{
    protected override string KeyName => "pk_support_line_responsible";

    protected override void ConfigureDataBase(EntityTypeBuilder<SupportLineResponsible> builder)
    {
        builder.ToTable("support_line_responsible", Options.Scheme);

        builder.Property(c => c.ObjectID).HasColumnName("object_id");
        builder.Property(c => c.ObjectClassID).HasColumnName("object_class_id");
        builder.Property(c => c.LineNumber).HasColumnName("line_number");
        builder.Property(c => c.OrganizationItemID).HasColumnName("organization_item_id");
        builder.Property(c => c.OrganizationItemClassID).HasColumnName("organization_item_class_id");
    }
}