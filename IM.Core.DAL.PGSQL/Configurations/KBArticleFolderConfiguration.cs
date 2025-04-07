using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.PGSQL.Configurations
{
    internal class KBArticleFolderConfiguration : KnowledgeBaseClassifierConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_kb_article_folder";
        protected override string UniqueIndexName => "uc_knowledge_base_classifier_name";
        protected override string ExpertForeignKeyName => "fk_knowledge_base_classifier_expert";
        protected override string ParentForeignKeyName => "fk_kb_article_folder_kb_article_folder";

        protected override void ConfigureDataBase(EntityTypeBuilder<KBArticleFolder> builder)
        {
            builder.ToTable("kb_article_folder", Options.Scheme);

            builder.Property(x => x.ID).HasColumnName("id");
            builder.Property(x => x.Name).HasColumnName("name");
            builder.Property(x => x.Note).HasColumnName("note");
            builder.Property(x => x.Visible).HasColumnName("visible");
            builder.Property(x => x.ParentID).HasColumnName("parent_id");
            builder.Property(x => x.ExpertID).HasColumnName("expert_id");
            builder.Property(x => x.UpdatePeriod).HasColumnName("update_period");
            builder.HasXminRowVersion(x => x.RowVersion);
        }
    }
}