using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class ObjectAccessConfiguration : ObjectAccessConfigurationBase
{
    protected override string KeyName => "PK_ObjectAccess";

    protected override void ConfigureDataBase(EntityTypeBuilder<ObjectAccess> builder)
    {
        builder.ToTable("ObjectAccess", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.ClassID).HasColumnName("ClassID");
        builder.Property(x => x.ObjectID).HasColumnName("ObjectID");
        builder.Property(x => x.OwnerID).HasColumnName("OwnerID");
        builder.Property(x => x.Propagate).HasColumnName("Propagate");
        builder.Property(x => x.Type).HasColumnName("Type");
    }
}
