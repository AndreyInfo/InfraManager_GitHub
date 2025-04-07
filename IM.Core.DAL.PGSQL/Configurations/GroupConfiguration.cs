using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

internal sealed class GroupConfiguration : GroupConfigurationBase
{
    protected override string KeyName => "pk_queue";

    protected override string DefaultValueID => "gen_random_uuid()";

    protected override string DefaultValueNote => "";

    protected override string UsersFK => "fk_queue_user_queue";

    protected override string UIName => "ui_name_quque";

    protected override void ConfigureDataBase(EntityTypeBuilder<Group> builder)
    {
        builder.ToTable("queue", Options.Scheme);

        builder.Property(c => c.IMObjID).HasColumnName("id");
        builder.Property(c => c.Name).HasColumnName("name");
        builder.HasXminRowVersion(e => e.RowVersion);
        builder.Property(c => c.Note).HasColumnName("note");
        builder.Property(c => c.Type).HasColumnName("type");
        builder.Property(c => c.ResponsibleID).HasColumnName("responsible_id");
    }
}
