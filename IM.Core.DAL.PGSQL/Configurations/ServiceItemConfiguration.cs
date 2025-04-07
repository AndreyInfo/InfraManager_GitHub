using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

internal sealed class ServiceItemConfiguration : ServiceItemConfigurationBase
{
    protected override string KeyName => "pk_service_item";

    protected override string NameUI => "ui_service_item_name_into_service";

    protected override string ServiceFK => "fk_service_item_service";

    protected override string FormForeignKey => "fk_service_item_form_id";

    protected override string ServiceIDIX => "ix_service_item_service_id";

    protected override void ConfigureDataBase(EntityTypeBuilder<ServiceItem> builder)
    {
        builder.ToTable("service_item", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.ServiceID).HasColumnName("service_id");
        builder.Property(x => x.Parameter).HasColumnName("parameter");
        builder.Property(x => x.ExternalID).HasColumnName("external_id");
        builder.Property(x => x.FormID).HasColumnName("form_id");
        builder.HasXminRowVersion(e => e.RowVersion);

        builder.Property(x => x.State)
            .HasColumnType("smallint")
            .HasColumnName("state");

        builder.Property(x => x.Note)
            .HasColumnType("text")
            .HasColumnName("note");
    }
}