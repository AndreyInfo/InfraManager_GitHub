using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Settings.UserFields;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

internal sealed class WorkOrderUserFieldNameConfiguration : WorkOrderUserFieldNameConfigurationBase
{
    protected override string KeyName => "work_order_additional_field_pkey";

    protected override void ConfigureDataBase(EntityTypeBuilder<WorkOrderUserFieldName> builder)
    {
        builder.ToTable("work_order_additional_field", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name");
    }
}