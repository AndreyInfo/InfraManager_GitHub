using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class KBTagConfiguration : IEntityTypeConfiguration<KBTag>
    {
        public void Configure(EntityTypeBuilder<KBTag> builder)
        {
            builder.ToTable("KBTag", "dbo");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .IsRequired()
                .HasColumnName("ID");
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(250)
                .HasColumnName("Name");
        }
    }
}
