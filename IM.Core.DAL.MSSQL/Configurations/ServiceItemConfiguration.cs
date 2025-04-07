using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal class ServiceItemConfiguration : ServiceItemConfigurationBase
{
    protected override string KeyName => "PK_ServiceItem";

    protected override string NameUI => "UI_Service_Item_Name_Into_Service";

    protected override string ServiceFK => "FK_ServiceItem_Service";

    protected override string FormForeignKey => "FK_ServiceItem_FormID";

    protected override string ServiceIDIX => "IX_ServiceItem_ServiceID";

    protected override void ConfigureDataBase(EntityTypeBuilder<ServiceItem> builder)
    {
        builder.ToTable("ServiceItem", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.Name).HasColumnName("Name");
        builder.Property(x => x.ExternalID).HasColumnName("ExternalID");
        builder.Property(x => x.FormID).HasColumnName("FormID");
        builder.Property(x => x.Parameter).HasColumnName("Parameter");
        builder.Property(x => x.ServiceID).HasColumnName("ServiceID");

        builder.Property(x => x.State)
            .HasColumnType("tinyint")
            .HasColumnName("State");

        builder.Property(x => x.Note)
            .HasColumnType("ntext")
            .HasColumnName("Note");

        builder.Property(x => x.RowVersion)
            .HasColumnType("timestamp")
            .IsRowVersion()
            .HasColumnName("RowVersion");
    }
}
