using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class TagConfiguration : TagConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_tag_id";
        protected override string IndexName => "unique_index_tag_name";

        protected override void ConfigureDatabase(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable("tag", Options.Scheme);

            builder.Property(t => t.ID).HasColumnName("id");
            builder.Property(t => t.Name).HasColumnName("name");
        }
    }
}
