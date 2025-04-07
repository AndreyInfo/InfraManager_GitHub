using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.EntityConfigurations;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal class ServiceUnitConfigurations : ServiceUnitConfigurationBase
{
    protected override string KeyName => "PK_ServiceUnit";

    protected override void ConfigureDataBase(EntityTypeBuilder<ServiceUnit> builder)
    {
        builder.ToTable("ServiceUnit", "dbo");

        builder.Property(c => c.ID).HasColumnName("ID");
        builder.Property(c => c.Name).HasColumnName("Name");
        builder.Property(c => c.ResponsibleID).HasColumnName("ResponsibleID");
        builder.Property(c => c.RowVersion).IsRowVersion().HasColumnName("RowVersion");
    }
}
