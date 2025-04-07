using IM.Core.DAL.Postgres;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.PGSQL.Configurations
{
    internal class KBArticleAccessConfiguration : IEntityTypeConfiguration<KBArticleAccess>
    {
        public void Configure(EntityTypeBuilder<KBArticleAccess> builder)
        {
            builder.ToTable("kb_article_access", Options.Scheme);
            builder.HasKey(x => x.ArticleAccessId);

            builder.Property(x => x.ArticleAccessId)
                .IsRequired()
                .HasColumnName("kb_article_access_id");
            builder.Property(x => x.ArticleAccessName)
                .IsRequired()
                .HasColumnName("kb_article_access_name");
        }
    }
}