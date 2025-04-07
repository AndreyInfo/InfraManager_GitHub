using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core = InfraManager.DAL.EntityConfigurations;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class KBArticleAccessListConfiguration : Core.KBArticleAccessListConfiguration
    {
        protected override string TableName => "KBArticleAccessList";

        protected override string TableSchema => "dbo";

        protected override void ConfigureDbProvider(EntityTypeBuilder<KBArticleAccessList> builder)
        {
            builder.Property(x => x.KbArticleID)
                .HasColumnName("KBArticleID");
            builder.Property(x => x.ObjectID)
                .HasColumnName("ObjectID");
            builder.Property(x => x.ObjectClass)
                .HasColumnName("ObjectClassID");
            builder.Property(x => x.ObjectName)
                .HasColumnName("ObjectName");
            builder.Property(x => x.WithSub)
                .HasColumnName("WithSub");
        }
    }
}
