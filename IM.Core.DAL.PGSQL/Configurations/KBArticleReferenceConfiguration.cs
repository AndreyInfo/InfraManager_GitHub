using IM.Core.DAL.Postgres;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.PGSQL.Configurations
{
    internal class KBArticleReferenceConfiguration : IEntityTypeConfiguration<KBArticleReference>
    {
        public void Configure(EntityTypeBuilder<KBArticleReference> builder)
        {
            builder.ToTable("kb_article_reference", Options.Scheme);
            builder.HasKey(x => new {x.ArticleId, x.ObjectClassID, x.ObjectId});

            ConfigureColumns(builder);
        }

        private static void ConfigureColumns(EntityTypeBuilder<KBArticleReference> builder)
        {
            builder.Property(x => x.ObjectId)
                .IsRequired()
                .HasColumnName("object_id");
            builder.Property(x => x.ObjectClassID)
                .IsRequired()
                .HasColumnName("object_class_id");
            builder.Property(x => x.ArticleId)
                .IsRequired()
                .HasColumnName("kb_article_id");
        }
    }
}