using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class UserAccountConfiguration : UserAccountConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_UserAccount_ID";
        protected override string IndexName => "Index_UserAccount_Name";

        protected override void ConfigureDatabase(EntityTypeBuilder<UserAccount> builder)
        {
            builder.ToTable("UserAccount", "dbo");

            builder.Property(ua => ua.ID).HasColumnName("ID");
            builder.Property(ua => ua.Name).HasColumnName("Name");
            builder.Property(ua => ua.Type).HasColumnName("Type");
            builder.Property(ua => ua.Login).HasColumnName("Login");
            builder.Property(ua => ua.Password).HasColumnName("Password");
            builder.Property(ua => ua.SSH_Passphrase).HasColumnName("SSH_Passphrase");
            builder.Property(ua => ua.SSH_PrivateKey).HasColumnName("SSH_PrivateKey");
            builder.Property(ua => ua.AuthenticationProtocol).HasColumnName("AuthenticationProtocol");
            builder.Property(ua => ua.AuthenticationKey).HasColumnName("AuthenticationKey");
            builder.Property(ua => ua.PrivacyProtocol).HasColumnName("PrivacyProtocol");
            builder.Property(ua => ua.PrivacyKey).HasColumnName("PrivacyKey");
            builder.Property(ua => ua.Removed).HasColumnName("Removed");
            builder.Property(ua => ua.CreateDate).HasColumnName("CreateDate");
            builder.Property(ua => ua.RemovedDate).HasColumnName("RemovedDate");
            builder.Property(ua => ua.RowVersion).IsRowVersion();
        }
    }
}
