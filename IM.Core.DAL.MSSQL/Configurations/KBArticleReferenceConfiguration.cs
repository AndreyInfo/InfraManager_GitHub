using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class KBArticleReferenceConfiguration : IEntityTypeConfiguration<KBArticleReference>
    {
        public void Configure(EntityTypeBuilder<KBArticleReference> builder)
        {
            builder.ToTable("KBArticleReference", "dbo");
            builder.HasKey(x => new { x.ArticleId, x.ObjectClassID, x.ObjectId });

            ConfigureColumns(builder);
        }

        private static void ConfigureColumns(EntityTypeBuilder<KBArticleReference> builder)
        {
            builder.Property(x => x.ObjectId)
                .IsRequired()
                .HasColumnName("ObjectID");
            builder.Property(x => x.ObjectClassID)
                .IsRequired()
                .HasColumnName("ObjectClassID");
            builder.Property(x => x.ArticleId)
                .IsRequired()
                .HasColumnName("KBArticleID");
        }
    }
}
