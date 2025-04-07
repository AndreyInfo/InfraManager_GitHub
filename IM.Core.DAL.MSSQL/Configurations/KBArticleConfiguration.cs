using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core = InfraManager.DAL.EntityConfigurations;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class KBArticleConfiguration : Core.KBArticleConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_KBArticle";

        protected override string NumberDefaultValueName => "NEXT VALUE FOR KBArticleNumber";

        protected override void ConfigureDatabase(EntityTypeBuilder<KBArticle> builder)
        {
            builder.ToTable("KBArticle", "dbo");
            builder.Property(x => x.ArticleTypeID)
                .HasColumnName("KBArticleTypeID");
            builder.Property(x => x.ArticleStatusID)
                .HasColumnName("KBArticleStatusID");
            builder.Property(x => x.ArticleAccessID)
                .HasColumnName("KBArticleAccessID");

            builder.HasMany(m => m.AccessList)
                .WithOne()
                .HasForeignKey(k => k.KbArticleID);
        }
    }
}
