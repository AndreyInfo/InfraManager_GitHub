using InfraManager.DAL.Documents;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class DocumentReferenceConfiguration : DocumentReferenceConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_DocumentReference";

        protected override string DocumentForeignKeyName => "FK_DocumentReference_Document";

        protected override void ConfigureProvider(EntityTypeBuilder<DocumentReference> builder)
        {
            builder.ToTable("DocumentReference", "dbo");
            builder.Property(e => e.DocumentID).HasColumnName("DocumentID");
            builder.Property(e => e.ObjectID).HasColumnName("ObjectID");
            builder.Property(e => e.ObjectClassID).HasColumnName("ObjectClassID");
        }
    }
}
