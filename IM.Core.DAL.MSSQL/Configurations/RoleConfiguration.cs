using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal class RoleConfiguration : RoleConfigurationBase
{
    protected override string KeyName => "PK_Role";

    protected override string DefaultValueID => "NEWID()";

    protected override string LifeCycleOperationFK => "FK_RoleLifeCycleStateOperation_Role";
    protected override string UI_Name => "UI_Role_Name";

    protected override void ConfigureDataBase(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Role", "dbo");
        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.Name).HasColumnName("Name");
        builder.Property(x => x.Note).HasColumnName("Note");
        builder.Property(x => x.RowVersion).IsRowVersion()
            .HasColumnType("timestamp")
            .HasColumnName("RowVersion");
    }
}
