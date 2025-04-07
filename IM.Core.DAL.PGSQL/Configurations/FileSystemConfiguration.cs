using IM.Core.DAL.Postgres;
using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class FileSystemConfiguration : IEntityTypeConfiguration<FileSystem>
    {
        public void Configure(EntityTypeBuilder<FileSystem> builder)
        {
            builder.ToTable("file_system", Options.Scheme);

            builder.HasKey(c => c.ID);
            builder.Property(c => c.ID).HasColumnName("id");
            builder.Property(c => c.Name).HasColumnName("name");
            builder.Property(c => c.ComplementaryID).HasColumnName("complementary_id");
            builder.HasXminRowVersion(x => x.RowVersion);
        }
    }
}