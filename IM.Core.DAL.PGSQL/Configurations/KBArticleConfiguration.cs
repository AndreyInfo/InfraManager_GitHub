using IM.Core.DAL.Postgres;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core = InfraManager.DAL.EntityConfigurations;

namespace InfraManager.DAL.PGSQL.Configurations
{
    internal class KBArticleConfiguration : Core.KBArticleConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_kb_article";

        protected override string NumberDefaultValueName => "nextval('kb_article_number')";

        protected override void ConfigureDatabase(EntityTypeBuilder<KBArticle> builder)
        {
            builder.ToTable("kb_article", Options.Scheme);

            builder.Property(x => x.ID)
                .HasColumnName("id");
            builder.Property(x => x.Name)
                .HasColumnName("name");
            builder.Property(x => x.UtcDateCreated)
                .HasColumnName("utc_date_created");
            builder.Property(x => x.UtcDateModified)
                .HasColumnName("utc_date_modified");
            builder.Property(x => x.AuthorID)
                .HasColumnName("author_id");
            builder.Property(x => x.ModifierID)
                .HasColumnName("modifier_id");
            builder.Property(x => x.Description)
                .HasColumnName("description");
            builder.Property(x => x.Solution)
                .HasColumnName("solution");
            builder.Property(x => x.Visible)
                .HasColumnName("visible");
            builder.Property(x => x.ArticleAccessID)
                .HasColumnName("kb_article_access_id");
            builder.Property(x => x.Number)
                .HasColumnName("number");
            builder.Property(x => x.ArticleStatusID)
                .HasColumnName("kb_article_status_id");
            builder.Property(x => x.HTMLDescription)
                .HasColumnName("html_description");
            builder.Property(x => x.HTMLSolution)
                .HasColumnName("html_solution");
            builder.Property(x => x.HTMLAlternativeSolution)
                .HasColumnName("html_alternative_solution");
            builder.Property(x => x.AlternativeSolution)
                .HasColumnName("alternative_solution");
            builder.Property(x => x.ArticleTypeID)
                .HasColumnName("kb_article_type_id");
            builder.Property(x => x.ExpertID)
                .HasColumnName("expert_id");
            builder.Property(x => x.UtcDateValidUntil)
                .HasColumnName("utc_date_valid_until");
            builder.Property(x => x.LifeCycleStateID)
                .HasColumnName("life_cycle_state_id");
            builder.Property(x => x.ViewsCount)
                .HasColumnName("views_count");
        }
    }
}