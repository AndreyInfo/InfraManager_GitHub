using IM.Core.DAL.Postgres;
using InfraManager.DAL.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.ToTable("document", Options.Scheme);
            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID)
                .IsRequired()
                .HasColumnName("id");
            builder.Property(x => x.InternalID)
                .HasDefaultValueSql("nextval('document_internal_id')")
                .ValueGeneratedOnAdd()
                .HasColumnName("internal_id");
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(250)
                .HasColumnName("name");
            builder.Property(x => x.Extension)
                .HasMaxLength(500)
                .HasColumnName("extension");
            builder.Property(x => x.Data)
                .HasColumnName("data");
            builder.Property(x => x.Location)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnName("location");
            builder.Property(x => x.DateModified)
                .IsRequired()
                .HasColumnName("date_modified");
            builder.Property(x => x.DateCreated)
                .IsRequired()
                .HasColumnName("date_created");
            builder.Property(x => x.DocumentTypeID)
                .HasColumnName("document_type_id");
            builder.Property(x => x.Note)
                .IsRequired()
                .HasColumnName("note");
            builder.Property(x => x.DocumentState)
                .HasColumnName("document_state");
            builder.Property(x => x.AuthorID)
                .IsRequired()
                .HasColumnName("author_id");
            builder.Property(x => x.OwnerID)
                .HasColumnName("owner_id");
            builder.Property(x => x.EncodedName)
                .HasColumnName("encoded_name")
                .IsRequired(false)
                .HasMaxLength(250);
        }
    }
}