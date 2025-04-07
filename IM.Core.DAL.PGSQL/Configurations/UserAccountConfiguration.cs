using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class UserAccountConfiguration : UserAccountConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_user_account_id";
        protected override string IndexName => "index_user_account_name";

        protected override void ConfigureDatabase(EntityTypeBuilder<UserAccount> builder)
        {
            builder.ToTable("user_account", Options.Scheme);

            builder.Property(ua => ua.ID).HasColumnName("id");
            builder.Property(ua => ua.Name).HasColumnName("name");
            builder.Property(ua => ua.Type).HasColumnName("type");
            builder.Property(ua => ua.Login).HasColumnName("login");
            builder.Property(ua => ua.Password).HasColumnName("password");
            builder.Property(ua => ua.SSH_Passphrase).HasColumnName("ssh_passphrase");
            builder.Property(ua => ua.SSH_PrivateKey).HasColumnName("ssh_private_key");
            builder.Property(ua => ua.AuthenticationProtocol).HasColumnName("authentication_protocol");
            builder.Property(ua => ua.AuthenticationKey).HasColumnName("authentication_key");
            builder.Property(ua => ua.PrivacyProtocol).HasColumnName("privacy_protocol");
            builder.Property(ua => ua.PrivacyKey).HasColumnName("privacy_key");
            builder.Property(ua => ua.Removed).HasColumnName("removed");
            builder.Property(ua => ua.CreateDate).HasColumnName("create_date").HasColumnType("datetime");
            builder.Property(ua => ua.RemovedDate).HasColumnName("removed_date").HasColumnType("datetime");
            builder.HasXminRowVersion(ua => ua.RowVersion);
        }
    }
}
