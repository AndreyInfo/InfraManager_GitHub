using IM.Core.DAL.Postgres;
using InfraManager.DAL.Documents;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations
{
    internal class DocumentReferenceConfiguration : DocumentReferenceConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_document_reference";

        protected override string DocumentForeignKeyName => "fk_document_reference_document";

        protected override void ConfigureProvider(EntityTypeBuilder<DocumentReference> builder)
        {
            builder.ToTable("document_reference", Options.Scheme);

            builder.Property(e => e.DocumentID).HasColumnName("document_id");
            builder.Property(e => e.ObjectID).HasColumnName("object_id");
            builder.Property(e => e.ObjectClassID).HasColumnName("object_class_id");
        }
    }
}