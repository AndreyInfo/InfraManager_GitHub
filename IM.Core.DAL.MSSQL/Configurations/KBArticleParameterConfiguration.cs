using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class KBArticleParameterConfiguration : IEntityTypeConfiguration<KBArticleParameter>
    {
        public void Configure(EntityTypeBuilder<KBArticleParameter> builder)
        {
            builder.ToTable("KBArticleParameter", "dbo");
            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID)
                .IsRequired()
                .HasColumnName("ID");
            builder.Property(x => x.Rating)
                .IsRequired()
                .HasColumnName("Rating");
            builder.Property(x => x.ReadCount)
                .IsRequired()
                .HasColumnName("ReadCount");
            builder.Property(x => x.UseCount)
                .IsRequired()
                .HasColumnName("UseCount");
            builder.Property(x => x.VoteCount)
                .IsRequired()
                .HasColumnName("VoteCount");
        }
    }
}
