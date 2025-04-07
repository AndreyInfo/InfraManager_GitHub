using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

internal sealed class ServiceConfiguration : ServiceConfigurationBase
{
    protected override string KeyName => "pk_service";

    protected override string NameUI => "ui_service_name_in_service_category";

    protected override string ServiceCategortFK => "fk_service_service_category";

    protected override string CategoryIDIX => "ix_service_service_category_id";

    protected override void ConfigureDataBase(EntityTypeBuilder<Service> builder)
    {
        builder.ToTable("service", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.Note).HasColumnName("note");
        builder.Property(x => x.Type).HasColumnName("type");
        builder.Property(x => x.State).HasColumnName("state");
        builder.Property(x => x.IconName).HasColumnName("icon_name");
        builder.Property(x => x.ExternalID).HasColumnName("external_id");
        builder.Property(x => x.CriticalityID).HasColumnName("criticality_id");
        builder.Property(x => x.CategoryID).HasColumnName("service_category_id");
        builder.Property(x => x.OrganizationItemClassID).HasColumnName("organization_item_class_id");
        builder.Property(x => x.OrganizationItemObjectID).HasColumnName("organization_item_object_id");
        builder.Property(x => x.OrganizationItemClassIDCustomer).HasColumnName("organization_item_class_id_customer");
        builder.Property(x => x.OrganizationItemObjectIDCustomer).HasColumnName("organization_item_object_id_customer");
        builder.HasXminRowVersion(e => e.RowVersion);
    }
}