using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.Accounts;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal class UserAccountTagConfiguration : UserAccountTagConfigurationBase
{
    protected override string KeyName => "PK_UserAccountTag";

    protected override string TagForeignKey => "FK_UserAccountTag_Tag_ID";

    protected override void ConfigureDataBase(EntityTypeBuilder<UserAccountTag> builder)
    {
        builder.ToTable("UserAccountTag", Options.Scheme);

        builder.Property(t => t.UserAccountID).HasColumnName("UserAccountID");
        builder.Property(t => t.TagID).HasColumnName("TagID");
    }
}
