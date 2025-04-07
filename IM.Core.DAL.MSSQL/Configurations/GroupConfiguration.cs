using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class GroupConfiguration : GroupConfigurationBase
{
    protected override string KeyName => "PK_Queue";

    protected override string DefaultValueID => "NEWID()";

    protected override string DefaultValueNote => "";

    protected override string UsersFK => "FK_QueueUser_Queue";

    protected override string UIName => "UI_Name_Quque";

    protected override void ConfigureDataBase(EntityTypeBuilder<Group> builder)
    {
        builder.ToTable("Queue", "dbo");

        builder.Property(c => c.IMObjID).HasColumnName("ID");
        builder.Property(c => c.Name).HasColumnName("Name");
        builder.Property(c => c.RowVersion)
            .IsRowVersion()
            .HasColumnType("timestamp")
            .HasColumnName("RowVersion");
        builder.Property(c => c.Note).HasColumnName("Note");
        builder.Property(c => c.Type).HasColumnType("tinyint").HasColumnName("Type");
        builder.Property(c => c.ResponsibleID).HasColumnName("ResponsibleID");
    }
}
