using IM.Core.DAL.Postgres;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations;

internal class AccessPermissionConfiguration : AccessPermissionConfigurationBase
{
    protected override string KeyName => "pk_access_permission";

    protected override void ConfigureDataBase(EntityTypeBuilder<AccessPermission> builder)
    {
        builder.ToTable("access_permission", Options.Scheme);

        builder.Property(c => c.ID).HasColumnName("id");
        builder.Property(c => c.ObjectClassID).HasColumnName("object_class_id");
        builder.Property(c => c.ObjectID).HasColumnName("object_id");
        builder.Property(c => c.Properties).HasColumnName("properties");
        builder.Property(c => c.Add).HasColumnName("add");
        builder.Property(c => c.Delete).HasColumnName("delete");
        builder.Property(c => c.Update).HasColumnName("update");
        builder.Property(c => c.AccessManage).HasColumnName("access_manage");
    }
}