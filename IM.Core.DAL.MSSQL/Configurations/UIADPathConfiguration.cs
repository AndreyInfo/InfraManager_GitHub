using Inframanager.DAL.ActiveDirectory.Import;
using Inframanager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class UIADPathConfiguration : UIADPathConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_UIADPath";

        protected override void ConfigureDatabase(EntityTypeBuilder<UIADPath> builder)
        {
            builder.ToTable("UIADPath", "dbo");

            builder.Property(x => x.ID).HasColumnName("ID");

            builder.Property(x => x.ADPathID).HasColumnName("AdPathID");

            builder.Property(x => x.ADSettingID).HasColumnName("ADSettingID");

            builder.Property(x => x.Path).HasColumnName("Path");

        }
    }
}