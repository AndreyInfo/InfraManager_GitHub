using InfraManager.DAL.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class TagConfigurationBase : IEntityTypeConfiguration<Tag>
    {
        protected abstract string PrimaryKeyName { get; }
        protected abstract string IndexName { get; }

        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.HasKey(t => t.ID).HasName(PrimaryKeyName);

            builder.Property(t => t.ID).ValueGeneratedOnAdd();
            builder.Property(t => t.Name).IsRequired(true).HasMaxLength(50);

            builder.HasIndex(t => t.Name, IndexName).IsUnique(true);

            ConfigureDatabase(builder);
        }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<Tag> builder);
    }
}
