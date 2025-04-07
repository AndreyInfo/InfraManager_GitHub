using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class TagConfiguration : TagConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_Tag_ID";
        protected override string IndexName => "UniqueIndex_Tag_Name";

        protected override void ConfigureDatabase(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable("Tag", "dbo");

            builder.Property(t => t.ID).HasColumnName("ID");
            builder.Property(t => t.Name).HasColumnName("Name");
        }
    }
}
