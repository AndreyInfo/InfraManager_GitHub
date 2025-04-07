using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal class SupportLineResponsibleConfiguration : SupportLineResponsibleConfigurationBase
{
    protected override string KeyName => "PK_SupportLineResponsible";

    protected override void ConfigureDataBase(EntityTypeBuilder<SupportLineResponsible> builder)
    {
        builder.ToTable("SupportLineResponsible", Options.Scheme);

        builder.Property(c => c.ObjectID).HasColumnName("ObjectID");
        builder.Property(c => c.ObjectClassID).HasColumnName("ObjectClassID");
        builder.Property(c => c.LineNumber).HasColumnName("LineNumber");
        builder.Property(c => c.OrganizationItemID).HasColumnName("OrganizationItemID");
        builder.Property(c => c.OrganizationItemClassID).HasColumnName("OrganizationItemClassID");
    }
}
