using IM.Core.DAL.Postgres;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

internal class OperationConfiguration : OperationConfigurationBase
{
    protected override string KeyName => "pk_operation";

    protected override void ConfigureDataBase(EntityTypeBuilder<Operation> builder)
    {
        builder.ToTable("operation", Options.Scheme);

        builder.Property(e => e.ID).HasColumnName("id");
        builder.Property(e => e.Name).HasColumnName("name");
        builder.Property(e => e.ClassID).HasColumnName("class_id");
        builder.Property(e => e.Description).HasColumnName("description");
        builder.Property(e => e.OperationName).HasColumnName("operation_name");
    }
}