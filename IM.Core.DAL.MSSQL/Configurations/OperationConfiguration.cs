using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal class OperationConfiguration : OperationConfigurationBase
{
    protected override string KeyName => "PK_Operation";

    protected override void ConfigureDataBase(EntityTypeBuilder<Operation> builder)
    {
        builder.ToTable("Operation", "dbo");

        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.ClassID).HasColumnName("ClassID");
        builder.Property(x => x.Name).HasColumnName("Name");
        builder.Property(x => x.OperationName).HasColumnName("OperationName");
        builder.Property(x => x.Description).HasColumnName("Description");
    }
}