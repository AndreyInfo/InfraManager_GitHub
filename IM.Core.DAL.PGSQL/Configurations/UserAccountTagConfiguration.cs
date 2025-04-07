using IM.Core.DAL.Postgres;
using InfraManager.DAL.Accounts;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

internal sealed class UserAccountTagConfiguration : UserAccountTagConfigurationBase
{
    protected override string KeyName => "pk_user_account_tag";

    protected override string TagForeignKey => "fk_user_account_tag_tag_id";

    protected override void ConfigureDataBase(EntityTypeBuilder<UserAccountTag> builder)
    {
        builder.ToTable("user_account_tag", Options.Scheme);

        builder.Property(t => t.UserAccountID).HasColumnName("user_account_id");
        builder.Property(t => t.TagID).HasColumnName("tag_id");

    }
}
