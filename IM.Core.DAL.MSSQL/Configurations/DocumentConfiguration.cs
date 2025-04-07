using InfraManager.DAL.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.ToTable("document", "dbo");
            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID)
                .IsRequired();
            builder.Property(x => x.InternalID)
                .HasColumnName("InternalID")
                .HasDefaultValueSql("NEXT VALUE FOR DocumentInternalID");
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(250);
            builder.Property(x => x.Extension)
                .HasMaxLength(500);
            builder.Property(x => x.Location)
                .IsRequired()
                .HasMaxLength(500);
            builder.Property(x => x.DateModified)
                .IsRequired();
            builder.Property(x => x.DateCreated)
                .IsRequired();
            builder.Property(x => x.Note)
                .IsRequired();
            builder.Property(x => x.AuthorID)
                .IsRequired();
            builder.Property(x => x.EncodedName)
                .HasColumnName("EncodedName")
                .IsRequired(false)
                .HasMaxLength(250);
        }
    }
}
