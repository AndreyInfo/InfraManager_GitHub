using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class KnowledgeBaseClassifierConfigurationBase : IEntityTypeConfiguration<KBArticleFolder>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string UniqueIndexName { get; }
    protected abstract string ExpertForeignKeyName { get; }
    protected abstract string ParentForeignKeyName { get; }
    
    
    public void Configure(EntityTypeBuilder<KBArticleFolder> builder)
    {
        builder.HasKey(e => e.ID).HasName(PrimaryKeyName);
        
        builder.HasIndex(x => x.Name, UniqueIndexName).IsUnique();

        builder.Property(x => x.Name).HasMaxLength(KBArticleFolder.MaxNameLength).IsRequired(true);
        builder.Property(x => x.Note).HasMaxLength(KBArticleFolder.MaxNoteLength).IsRequired(true);

        builder.HasOne(x => x.Parent).WithMany().HasForeignKey(x => x.ParentID).HasConstraintName(ParentForeignKeyName);
        builder.HasOne(x => x.Expert).WithMany().HasForeignKey(x => x.ExpertID).HasConstraintName(ExpertForeignKeyName);

        ConfigureDataBase(builder);
    }
    
    protected abstract void ConfigureDataBase(EntityTypeBuilder<KBArticleFolder> builder);
}