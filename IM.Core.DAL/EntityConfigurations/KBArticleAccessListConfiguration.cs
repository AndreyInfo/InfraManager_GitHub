using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class KBArticleAccessListConfiguration : TableConfigurationBase<KBArticleAccessList>, IEntityTypeConfiguration<KBArticleAccessList>
    {
        protected override void ConfigureCommon(EntityTypeBuilder<KBArticleAccessList> builder)
        {
            builder.HasKey(x => new { x.KbArticleID, x.ObjectID });

            builder.Property(x => x.ObjectClass)
                .IsRequired();
            builder.Property(x => x.ObjectName)
                .HasMaxLength(510);
            builder.Property(x => x.WithSub)
                .IsRequired();
        }
    }
}
