using IM.Core.DAL.Postgres;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.PGSQL.Configurations
{
    internal class KBTagConfiguration : IEntityTypeConfiguration<KBTag>
    {
        public void Configure(EntityTypeBuilder<KBTag> builder)
        {
            builder.ToTable("kb_tag", Options.Scheme);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .IsRequired()
                .HasColumnName("id");
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(250)
                .HasColumnName("name");
        }
    }
}