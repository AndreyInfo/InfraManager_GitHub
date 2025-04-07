using IM.Core.DAL.Postgres;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.PGSQL.Configurations
{
    internal class KBArticleTypeConfiguration : IEntityTypeConfiguration<KBArticleType>
    {
        public void Configure(EntityTypeBuilder<KBArticleType> builder)
        {
            builder.ToTable("kb_article_type", Options.Scheme);
            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID)
                .IsRequired()
                .HasColumnName("id");
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnName("name");
        }
    }
}