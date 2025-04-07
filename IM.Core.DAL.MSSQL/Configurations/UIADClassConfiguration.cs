using Inframanager.DAL.ActiveDirectory.Import;
using Inframanager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class UIADClassConfiguration : UIADClassConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_UIADClass";

        protected override void ConfigureDatabase(EntityTypeBuilder<UIADClass> builder)
        {
            builder.ToTable("UIADClass", "dbo");

            builder.Property(x => x.ID).HasColumnName("ID");

            builder.Property(x => x.Name).HasColumnName("Name");
        }
    }
}