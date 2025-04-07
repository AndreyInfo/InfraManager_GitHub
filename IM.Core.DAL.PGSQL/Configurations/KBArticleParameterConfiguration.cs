using IM.Core.DAL.Postgres;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.PGSQL.Configurations
{
    internal class KBArticleParameterConfiguration : IEntityTypeConfiguration<KBArticleParameter>
    {
        public void Configure(EntityTypeBuilder<KBArticleParameter> builder)
        {
            builder.ToTable("kb_article_parameter", Options.Scheme);
            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID)
                .IsRequired()
                .HasColumnName("id");
            builder.Property(x => x.Rating)
                .IsRequired()
                .HasColumnName("rating");
            builder.Property(x => x.ReadCount)
                .IsRequired()
                .HasColumnName("read_count");
            builder.Property(x => x.UseCount)
                .IsRequired()
                .HasColumnName("use_count");
            builder.Property(x => x.VoteCount)
                .IsRequired()
                .HasColumnName("vote_count");
        }
    }
}