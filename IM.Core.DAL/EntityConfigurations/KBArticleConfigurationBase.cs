using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class KBArticleConfigurationBase : IEntityTypeConfiguration<KBArticle>
    {
        protected abstract string PrimaryKeyName { get; }

        protected abstract string NumberDefaultValueName { get; }

        public void Configure(EntityTypeBuilder<KBArticle> builder)
        {
            builder.HasKey(x => x.ID)
                   .HasName(PrimaryKeyName);

            builder.Property(x => x.ID)
                .IsRequired();
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(250);
            builder.Property(x => x.UtcDateCreated)
                .IsRequired();
            builder.Property(x => x.UtcDateModified)
                .IsRequired();
            builder.Property(x => x.AuthorID)
                .IsRequired();
            builder.Property(x => x.ModifierID)
                .IsRequired();
            builder.Property(x => x.Description)
                .IsRequired();
            builder.Property(x => x.Solution)
                .IsRequired();
            builder.Property(x => x.Visible)
                .IsRequired();
            builder.Property(x => x.ArticleAccessID)
                .IsRequired();
            builder.Property(x => x.Number)
                .HasDefaultValueSql(NumberDefaultValueName)
                .ValueGeneratedOnAdd();
            builder.Property(x => x.ArticleStatusID)
                .IsRequired();
            builder.Property(x => x.HTMLDescription)
                .IsRequired();
            builder.Property(x => x.HTMLSolution)
                .IsRequired();
            builder.Property(x => x.HTMLAlternativeSolution)
                .IsRequired();
            builder.Property(x => x.AlternativeSolution)
                .IsRequired();
            builder.Property(x => x.ArticleTypeID)
                .IsRequired();
            builder.Property(x => x.ViewsCount)
                .IsRequired()
                .HasDefaultValue(0);

            ConfigureDatabase(builder);
        }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<KBArticle> builder);
    }
}
