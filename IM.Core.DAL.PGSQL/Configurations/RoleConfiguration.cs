using IM.Core.DAL.Postgres;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

internal class RoleConfiguration : RoleConfigurationBase
{
    protected override string KeyName => "pk_role";

    protected override string DefaultValueID => "gen_random_uuid()";

    protected override string LifeCycleOperationFK => "fk_role_life_cycle_state_operation_role";
    
    protected override string UI_Name => "ui_role_name";

    protected override void ConfigureDataBase(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("role", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.Note).HasColumnName("note");
        builder.HasXminRowVersion(e => e.RowVersion);
    }
}