using InfraManager.DAL.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class UserAccountTagConfigurationBase : IEntityTypeConfiguration<UserAccountTag>
{
    protected abstract string KeyName { get; }
    protected abstract string TagForeignKey { get; }

    public void Configure(EntityTypeBuilder<UserAccountTag> builder)
    {
        builder
            .HasKey(m => new { m.TagID, m.UserAccountID });

        builder
            .HasOne(x => x.Tag)
            .WithMany(p => p.UserAccountTag)
            .HasForeignKey(x => x.TagID)
            .HasConstraintName(TagForeignKey);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<UserAccountTag> builder);
}