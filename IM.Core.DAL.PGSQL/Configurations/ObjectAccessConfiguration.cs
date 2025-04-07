using IM.Core.DAL.Postgres;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

internal sealed class ObjectAccessConfiguration : ObjectAccessConfigurationBase
{
    protected override string KeyName => "object_access_pkey";

    protected override void ConfigureDataBase(EntityTypeBuilder<ObjectAccess> builder)
    {
        builder.ToTable("object_access", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id");
        builder.Property(x => x.ClassID).HasColumnName("class_id");
        builder.Property(x => x.ObjectID).HasColumnName("object_id");
        builder.Property(x => x.OwnerID).HasColumnName("owner_id");
        builder.Property(x => x.Propagate).HasColumnName("propagate");
        builder.Property(x => x.Type).HasColumnName("type");
    }
}