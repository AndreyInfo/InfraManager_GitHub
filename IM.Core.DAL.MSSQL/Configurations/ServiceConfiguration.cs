using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class ServiceConfiguration : ServiceConfigurationBase
{
    protected override string KeyName => "PK_Service";

    protected override string NameUI => "UI_ServiceName_In_ServiceCategory";

    protected override string ServiceCategortFK => "FK_Service_ServiceCategory";

    protected override string CategoryIDIX => "IX_Service_ServiceCategoryID";

    protected override void ConfigureDataBase(EntityTypeBuilder<Service> builder)
    {
        builder.ToTable("Service", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.Name).HasColumnName("Name");
        builder.Property(x => x.Note).HasColumnName("Note");
        builder.Property(x => x.Type).HasColumnName("Type");
        builder.Property(x => x.State).HasColumnName("State");
        builder.Property(x => x.IconName).HasColumnName("IconName");
        builder.Property(x => x.ExternalID).HasColumnName("ExternalID");
        builder.Property(x => x.CriticalityID).HasColumnName("CriticalityID");
        builder.Property(c => c.CategoryID).HasColumnName("ServiceCategoryID");
        builder.Property(x => x.OrganizationItemClassID).HasColumnName("OrganizationItemClassID");
        builder.Property(x => x.OrganizationItemObjectID).HasColumnName("OrganizationItemObjectID");
        builder.Property(x => x.OrganizationItemClassIDCustomer).HasColumnName("OrganizationItemClassIDCustomer");
        builder.Property(x => x.OrganizationItemObjectIDCustomer).HasColumnName("OrganizationItemObjectIDCustomer");

        builder.Property(x => x.RowVersion)
            .HasColumnType("timestamp")
            .IsRowVersion()
            .HasColumnName("RowVersion");
    }
}
