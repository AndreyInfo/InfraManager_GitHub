using IM.Core.DAL.Postgres;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core = InfraManager.DAL.EntityConfigurations;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class KBArticleAccessListConfiguration : Core.KBArticleAccessListConfiguration
    {
        protected override string TableName => "kb_article_access_list";

        protected override string TableSchema => Options.Scheme;

        protected override void ConfigureDbProvider(EntityTypeBuilder<KBArticleAccessList> builder)
        {
            builder.Property(x => x.KbArticleID)
                .HasColumnName("kb_article_id");
            builder.Property(x => x.ObjectID)
                .HasColumnName("object_id");
            builder.Property(x => x.ObjectClass)
                .HasColumnName("object_class_id");
            builder.Property(x => x.ObjectName)
                .HasColumnName("object_name");
            builder.Property(x => x.WithSub)
                .HasColumnName("with_sub");
        }
    }
}