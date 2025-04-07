using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class KnowledgeBaseArticleStatusConfigurationBase : IEntityTypeConfiguration<KnowledgeBaseArticleStatus>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string UI_Name { get; }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<KnowledgeBaseArticleStatus> builder);
    
    public void Configure(EntityTypeBuilder<KnowledgeBaseArticleStatus> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

        builder.Property(x => x.ID)
            .IsRequired(true);
        builder.Property(x => x.Name)
            .IsRequired(true)
            .HasMaxLength(250);
        
        builder.HasIndex(x => x.Name).HasDatabaseName(UI_Name).IsUnique();
        ConfigureDatabase(builder);
    }
}