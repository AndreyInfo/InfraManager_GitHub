using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class KBArticleFolderConfiguration : KnowledgeBaseClassifierConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_KBArticleFolder";
        protected override string UniqueIndexName => "UC_Knowledge_Base_Classifier_Name";
        protected override string ExpertForeignKeyName => "FK_Knowledge_Base_Classifier_Expert";
        protected override string ParentForeignKeyName => "FK_KBArticleFolder_KBArticleFolder";

        protected override void ConfigureDataBase(EntityTypeBuilder<KBArticleFolder> builder)
        {
            builder.ToTable("KBArticleFolder", Options.Scheme);
            
            builder.Property(x => x.ID).HasColumnName("ID");
            builder.Property(x => x.Name).HasColumnName("Name");
            builder.Property(x => x.Note).HasColumnName("Note");
            builder.Property(x => x.Visible).HasColumnName("Visible");
            builder.Property(x => x.ParentID).HasColumnName("ParentID");
            builder.Property(x => x.RowVersion).HasColumnName("RowVersion").IsRowVersion();
        }
    }
}
