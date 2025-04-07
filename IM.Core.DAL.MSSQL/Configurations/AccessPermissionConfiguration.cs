using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal class AccessPermissionConfiguration : AccessPermissionConfigurationBase
{
    protected override string KeyName => "PK_AccessPermission";

    protected override void ConfigureDataBase(EntityTypeBuilder<AccessPermission> builder)
    {
        builder.ToTable("AccessPermission", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.ObjectClassID).HasColumnName("ObjectClassID");
        builder.Property(c => c.ObjectID).HasColumnName("ObjectID");
        builder.Property(c => c.Properties).HasColumnName("Properties");
        builder.Property(c => c.Add).HasColumnName("Add");
        builder.Property(c => c.Delete).HasColumnName("Delete");
        builder.Property(c => c.Update).HasColumnName("Update");
        builder.Property(c => c.AccessManage).HasColumnName("AccessManage");
    }
}