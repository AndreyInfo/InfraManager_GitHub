using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class KBArticleAccessConfiguration : IEntityTypeConfiguration<KBArticleAccess>
    {
        public void Configure(EntityTypeBuilder<KBArticleAccess> builder)
        {
            builder.ToTable("KBArticleAccess", "dbo");
            builder.HasKey(x => x.ArticleAccessId);

            builder.Property(x => x.ArticleAccessId)
                .IsRequired()
                .HasColumnName("KBArticleAccessID");
            builder.Property(x => x.ArticleAccessName)
                .IsRequired()
                .HasColumnName("KBArticleAccessName");
        }
    }
}
