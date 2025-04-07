using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using InfraManager.DAL.Accounts;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class UserAccountConfigurationBase : IEntityTypeConfiguration<UserAccount>
    {
        public void Configure(EntityTypeBuilder<UserAccount> builder)
        {
            builder.HasKey(ua => ua.ID).HasName(PrimaryKeyName);
            builder.Property(ua => ua.Type).IsRequired(true);
            builder.Property(ua => ua.Name).IsRequired(true).HasMaxLength(50);
            builder.Property(ua => ua.Login).IsRequired(false).HasMaxLength(50);
            builder.Property(ua => ua.Password).IsRequired(false).HasMaxLength(500);
            builder.Property(ua => ua.SSH_Passphrase).IsRequired(false).HasMaxLength(500);
            builder.Property(ua => ua.SSH_PrivateKey).IsRequired(false).HasMaxLength(500);
            builder.Property(ua => ua.AuthenticationProtocol).IsRequired(true);
            builder.Property(ua => ua.AuthenticationKey).IsRequired(false).HasMaxLength(500);
            builder.Property(ua => ua.PrivacyProtocol).IsRequired(true);
            builder.Property(ua => ua.PrivacyKey).IsRequired(false).HasMaxLength(500);
            builder.Property(ua => ua.Removed).IsRequired(true);
            builder.Property(ua => ua.CreateDate).IsRequired(true);
            builder.Property(ua => ua.RemovedDate).IsRequired(false);
            builder.IsMarkableForDelete();

            builder.HasIndex(ua => ua.Name).HasDatabaseName(IndexName);

            ConfigureDatabase(builder);
        }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<UserAccount> builder);

        protected abstract string PrimaryKeyName { get; }
        protected abstract string IndexName { get; }
    }
}
