using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class KBArticleStatusConfiguration : KnowledgeBaseArticleStatusConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_kb_article_status";
        protected override string UI_Name => "ui_kb_article_status_name";

        protected override void ConfigureDatabase(EntityTypeBuilder<KnowledgeBaseArticleStatus> builder)
        {
            builder.ToTable("kb_article_status", Options.Scheme);
            
            builder.Property(x => x.ID).HasColumnName("id");
            builder.Property(x => x.Name).HasColumnName("name");
            builder.HasXminRowVersion(x => x.RowVersion);
        }
    }
}