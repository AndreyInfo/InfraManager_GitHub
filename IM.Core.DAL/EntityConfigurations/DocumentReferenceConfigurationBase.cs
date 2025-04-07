using InfraManager.DAL.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class DocumentReferenceConfigurationBase : IEntityTypeConfiguration<DocumentReference>
    {
        protected abstract string PrimaryKeyName { get; }
        protected abstract string DocumentForeignKeyName { get; }

        public void Configure(EntityTypeBuilder<DocumentReference> builder)
        {
            builder.HasKey(docRef => new { docRef.DocumentID, docRef.ObjectID }); // TODO: Добавить ObjectClassID в первичный ключ
            builder
                .HasOne(docRef => docRef.Document)
                .WithMany()
                .HasForeignKey(docRef => docRef.DocumentID)
                .HasConstraintName(DocumentForeignKeyName);
            ConfigureProvider(builder);
        }

        protected abstract void ConfigureProvider(EntityTypeBuilder<DocumentReference> builder);
    }
}
