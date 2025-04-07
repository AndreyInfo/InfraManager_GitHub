using InfraManager.DAL;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations
{
    internal class ObjectIconConfiguration : ObjectIconConfigurationBase
    {
        protected override string UniqueKeyObjectIDName => "UK_ObjectIcon_ObjectID";

        protected override void ConfigureDataProvider(EntityTypeBuilder<ObjectIcon> builder)
        {
            builder.ToTable("ObjectIcon", "dbo");

            builder.Property(x => x.ID).HasColumnName("ID");
            builder.Property(x => x.ObjectID).HasColumnName("ObjectID");
            builder.Property(x => x.ObjectClassID).HasColumnName("ObjectClassID");
            builder.Property(x => x.Name).HasColumnName("Name");
            builder.Property(x => x.Content).HasColumnName("Content");
        }
    }
}
