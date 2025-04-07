using InfraManager.DAL.Settings.UserFields;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.EntityConfigurations;
public abstract class WorkOrderUserFieldNameConfigurationBase : IEntityTypeConfiguration<WorkOrderUserFieldName>
{
    protected abstract string KeyName { get; }

    public void Configure(EntityTypeBuilder<WorkOrderUserFieldName> builder)
    {
        builder.HasKey(c => c.ID).HasName(KeyName);

        builder.Property(c => c.Name).HasMaxLength(250).IsRequired(true);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<WorkOrderUserFieldName> builder);
}