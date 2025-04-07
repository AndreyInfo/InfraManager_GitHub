using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class KBArticleStatusConfiguration : KnowledgeBaseArticleStatusConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_KBArticleStatus";
        protected override string UI_Name => "UI_KBArticleStatus_Name";
        
        protected override void ConfigureDatabase(EntityTypeBuilder<KnowledgeBaseArticleStatus> builder)
        {
            builder.ToTable("KBArticleStatus", "dbo");
            
            builder.Property(x => x.ID).HasColumnName("ID");
            builder.Property(x => x.Name).HasColumnName("Name");
            builder.Property(x => x.RowVersion).HasColumnName("RowVersion").IsRowVersion();
        }
    }
}
