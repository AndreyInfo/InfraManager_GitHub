using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class KBArticleTypeConfiguration : IEntityTypeConfiguration<KBArticleType>
    {
        public void Configure(EntityTypeBuilder<KBArticleType> builder)
        {
            builder.ToTable("KBArticleType", "dbo");
            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID)
                .IsRequired()
                .HasColumnName("ID");
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnName("Name");
        }
    }
}
