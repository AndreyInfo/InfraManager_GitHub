using InfraManager.DAL;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations
{
    internal class ObjectIconConfiguration : ObjectIconConfigurationBase
    {
        protected override string UniqueKeyObjectIDName => "uk_object_icon_object_id";

        protected override void ConfigureDataProvider(EntityTypeBuilder<ObjectIcon> builder)
        {
            builder.ToTable("object_icon", "im");

            builder.Property(x => x.ID).HasColumnName("id");
            builder.Property(x => x.ObjectID).HasColumnName("object_id");
            builder.Property(x => x.ObjectClassID).HasColumnName("object_class_id");
            builder.Property(x => x.Name).HasColumnName("name");
            builder.Property(x => x.Content).HasColumnName("content");
        }
    }
}
