using IM.Core.DAL.Postgres;
using Inframanager.DAL.ActiveDirectory.Import;
using Inframanager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class UIADPathConfiguration : UIADPathConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_uiad_path";

        protected override void ConfigureDatabase(EntityTypeBuilder<UIADPath> builder)
        {
            builder.ToTable("uiad_path", Options.Scheme);

            builder.Property(x => x.ID).HasColumnName("id");

            builder.Property(x => x.ADPathID).HasColumnName("ad_path_id");

            builder.Property(x => x.ADSettingID).HasColumnName("ad_setting_id");

            builder.Property(x => x.Path).HasColumnName("path");
        }
    }
}