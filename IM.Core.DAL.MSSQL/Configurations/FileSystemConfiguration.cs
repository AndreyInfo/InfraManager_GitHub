using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class FileSystemConfiguration : IEntityTypeConfiguration<FileSystem>
    {
        public void Configure(EntityTypeBuilder<FileSystem> builder)
        {
            builder.ToTable("FileSystem", "dbo");

            builder.HasKey(c => c.ID);
            builder.Property(c => c.ID)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWID()")
                .HasColumnName("ID");
            builder.Property(c => c.Name)
                .HasColumnName("Name");
            builder.Property(c => c.ComplementaryID)
                .HasColumnName("ComplementaryID");
            builder.Property(c => c.RowVersion)
                .IsRowVersion()
                .HasColumnName("RowVersion"); ;

        }
    }
}
